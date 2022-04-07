using People;
using Company;
using Users;
using Relation;
using FileTools;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace ExplorationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]  this requires authentication on the whole controller
    public class ExplorationController : ControllerBase
    {
        #region DependencyInjection
        private readonly ILogger<ExplorationController> _logger;
        private ICitizenCache _citizenCache;
        private ICompanyCache _companyCache;
        private IUserCache _userCache;
        private IRelationshipCache _relationshipCache;
        private IFileTool _fileTool;

        public ExplorationController(
            ILogger<ExplorationController> logger,
            ICitizenCache citizenCache,
            ICompanyCache companyCache,
            IUserCache userCache,
            IRelationshipCache relationshipCache,
            IFileTool fileTool)
        {
            _logger = logger;
            _citizenCache = citizenCache;
            _companyCache = companyCache;
            _userCache = userCache;
            _relationshipCache = relationshipCache;
            _fileTool = fileTool;
        }
        #endregion

        private UserDto userDto = new();

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            if (_userCache.CheckforUser(request.UserName)) return BadRequest("username exists");
            userDto = CreatePasswordHash(request, out byte[] hash, out byte[] salt);
            _userCache.CreateNewUser(request.UserName, hash, salt, (CitizenCache)_citizenCache, (CompanyCache)_companyCache);
            return Ok("User Created");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login (UserDto request)
        {
            if (!_userCache.Users.ContainsKey(request.UserName)) return NotFound("User not found");
            var userPass = _userCache.Users[request.UserName].PasswordHash;
            var userSalt = _userCache.Users[request.UserName].PasswordSalt;
            if (!VerifyPassword(request.UserName, request.Password, userPass, userSalt)) return BadRequest("wrong password");
            string token = CreateToken(_userCache.Users[request.UserName]);
            return Ok(token);
        }
        private UserDto CreatePasswordHash(UserDto userDto, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userDto.Password));
                userDto.Password = string.Empty;
            }
            return userDto;
        }

        private bool VerifyPassword(string username, string password, byte[] hash, byte[] salt)
        {
            if (!_userCache.CheckforUser(username)) throw new Exception("user does not exist");
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Player")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_fileTool.LoginKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        [HttpGet("save", Name = "Advance & Save")]
        public async Task<string> Save()
        {
            DateTime currentDateTime = DateTime.Now;
            TimeSpan timeSpan = currentDateTime - _userCache.LastSave;
            double interval = timeSpan.TotalSeconds;
            foreach (User user in _userCache.Users.Values)
            {
                user.GainTimePoints(interval);
            }
            _userCache.LastSave = currentDateTime;
            await _fileTool.StoreCitizens((CitizenCache)_citizenCache);
            await _fileTool.StoreCompanies((CompanyCache)_companyCache);
            await _fileTool.StoreRelationshipCache((RelationshipCache)_relationshipCache);
            await _fileTool.StoreUsers((UserCache)_userCache);
            return "Everything saved!  Beep Beep Woop Woop";
        }

        [HttpGet("company/{username}", Name = "Company Get"), Authorize(Roles = "Player")]  //"Allow Anonymous"  will allow for everyone
        public string InnerGet(string username)
        {
            if (_userCache.Users.TryGetValue(username, out User user)) return _companyCache.PlayerCompanies[user.CompanyId].Describe();
            else return "That company doesn't exist";
        }
    }
}