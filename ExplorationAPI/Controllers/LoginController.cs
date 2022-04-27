using ExplorationAPI.Services.LoginService;
using ExplorationAPI.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExplorationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        #region Dependency Injection
        private readonly ILogger<ExplorationController> _logger;
        private ICitizenCache _citizenCache;
        private ICompanyCache _companyCache;
        private IUserCache _userCache;
        private IFileTool _fileTool;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;

        public LoginController(
            ILogger<ExplorationController> logger,
            ICitizenCache citizenCache,
            ICompanyCache companyCache,
            IUserCache userCache,
            IFileTool fileTool,
            IUserService userService,
            ILoginService loginService)
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

        UserDto userDto = new UserDto();

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            if (_userCache.CheckforUser(request.UserName)) return BadRequest("username exists");
            userDto = _loginService.CreatePasswordHash(request, out byte[] hash, out byte[] salt);
            _userCache.CreateNewUser(request.UserName, hash, salt, (CitizenCache)_citizenCache, (CompanyCache)_companyCache);
            return Ok("User Created");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            if (!_userCache.Users.ContainsKey(request.UserName)) return NotFound("User not found");
            var userPass = _userCache.Users[request.UserName].PasswordHash;
            var userSalt = _userCache.Users[request.UserName].PasswordSalt;
            if (!_loginService.VerifyPassword(request.UserName, request.Password, userPass, userSalt, _userCache)) return BadRequest("wrong password");
            string token = _loginService.CreateToken(_userCache.Users[request.UserName], _fileTool);
            return Ok(token);
        }
    }
}
