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
using ExplorationAPI.Services.UserServices;
using ExplorationAPI.Services.LoginService;

namespace ExplorationAPI.Controllers
{
    [ApiController, Authorize(Roles = "Player,Admin")]
    [Route("[controller]")]
    public class ExplorationController : ControllerBase
    {
        #region DependencyInjection
        private readonly ILogger<ExplorationController> _logger;
        private ICitizenCache _citizenCache;
        private ICompanyCache _companyCache;
        private IUserCache _userCache;
        private IRelationshipCache _relationshipCache;
        private IFileTool _fileTool;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;

        public ExplorationController(
            ILogger<ExplorationController> logger,
            ICitizenCache citizenCache,
            ICompanyCache companyCache,
            IUserCache userCache,
            IRelationshipCache relationshipCache,
            IFileTool fileTool,
            IUserService userService,
            ILoginService loginService)
        {
            _logger = logger;
            _citizenCache = citizenCache;
            _companyCache = companyCache;
            _userCache = userCache;
            _relationshipCache = relationshipCache;
            _fileTool = fileTool;
            _userService = userService;
            _loginService = loginService;
        }
        #endregion
        
        [HttpGet("company", Name = "Company Get")]  //"Allow Anonymous"  will allow for everyone
        public string Company()
        {
            var userName = _userService.GetUserName();
            if (_userCache.Users.TryGetValue(userName, out User user)) return _companyCache.PlayerCompanies[user.CompanyId].Describe();
            else return "That company doesn't exist";
        }
        [HttpPost("walk", Name = "Walk down the Road")]  //"Allow Anonymous"  will allow for everyone
        public string Walk()
        {
            var userName = _userService.GetUserName();
            if (_userCache.Users.TryGetValue(userName, out User user)) return ExplorationAPIMethods.Walk(user, _companyCache.PlayerCompanies[user.CompanyId]);
            else return "That company doesn't exist";
        }
        [HttpPost("progress", Name = "Progress Event")]  //"Allow Anonymous"  will allow for everyone
        public string ProgressEvent(int choice)
        {
            var userName = _userService.GetUserName();
            if (_userCache.Users.TryGetValue(userName, out User user)) return ExplorationAPIMethods.ProgressEvent(_companyCache.PlayerCompanies[user.CompanyId], choice);
            else return "That company doesn't exist";
        }
    }
}