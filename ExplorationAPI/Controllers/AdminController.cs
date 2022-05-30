using ExplorationAPI.Services.LoginService;
using ExplorationAPI.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExplorationAPI.Controllers
{
    [ApiController, Authorize(Roles = "Admin") ]
    [Route("admin/[controller]")]
    public class AdminController : ControllerBase
    {
        #region Dependency Injection
        private readonly ILogger<ExplorationController> _logger;
        private ICitizenCache _citizenCache;
        private ICompanyCache _companyCache;
        private IUserCache _userCache;
        private IFileTool _fileTool;
        //private IRelationshipCache _relationshipCache;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;

        public AdminController(
            ILogger<ExplorationController> logger,
            ICitizenCache citizenCache,
            ICompanyCache companyCache,
            IUserCache userCache,
            IFileTool fileTool,
            IUserService userService,
            ILoginService loginService
            )
        {
            _logger = logger;
            _citizenCache = citizenCache;
            _companyCache = companyCache;
            _userCache = userCache;
            _fileTool = fileTool;
            _userService = userService;
            _loginService = loginService;
        }
        #endregion

        AdminDto adminDto = new AdminDto();

        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<User>> RegisterAdmin(AdminDto request)
        {
            if (!(request.AdminCheck == _fileTool.AdminCheck)) return BadRequest("Admin check doesnt match");
            UserDto userDto = new UserDto();
            userDto.UserName = request.UserName;
            userDto.Password = request.Password;
            if (_userCache.CheckforUser(request.UserName)) return BadRequest("username exists");
            userDto = _loginService.CreatePasswordHash(userDto, out byte[] hash, out byte[] salt);
            _userCache.CreateNewUser(request.UserName, hash, salt, (CitizenCache)_citizenCache, (CompanyCache)_companyCache);
            _userCache.Users[request.UserName].Admin = true;
            return Ok("Admin Created");
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<string>> LoginAdmin(AdminDto request)
        {
            if (!_userCache.Users.ContainsKey(request.UserName)) return NotFound("User not found");
            if (!_userCache.Users[request.UserName].Admin) return BadRequest("Not an Admin");
            var userPass = _userCache.Users[request.UserName].PasswordHash;
            var userSalt = _userCache.Users[request.UserName].PasswordSalt;
            if (!_loginService.VerifyPassword(request.UserName, request.Password, userPass, userSalt, _userCache)) return BadRequest("wrong password");
            string token = _loginService.CreateToken(_userCache.Users[request.UserName], _fileTool, true);
            return Ok(token);
        }

        [HttpGet("save", Name = "Advance & Save")]
        public async Task<string> Save()
        {
            long interval = 10000;
            _companyCache.Time += interval;
            await _fileTool.StoreCitizens((CitizenCache)_citizenCache);
            await _fileTool.StoreCompanies((CompanyCache)_companyCache);
            await _fileTool.StoreUsers((UserCache)_userCache);
            return "Everything saved!  Beep Beep Woop Woop";
        }
    }
}
