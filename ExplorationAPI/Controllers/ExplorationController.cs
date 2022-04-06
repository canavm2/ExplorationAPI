using People;
using Company;
using Users;
using Relation;
using Microsoft.AspNetCore.Mvc;

namespace ExplorationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExplorationController : ControllerBase
    {
        #region DependencyInjection
        private readonly ILogger<ExplorationController> _logger;
        private ICitizenCache _citizenCache;
        private ICompanyCache _companyCache;
        private IUserCache _userCache;
        private IRelationshipCache _relationshipCache;

        public ExplorationController(
            ILogger<ExplorationController> logger,
            ICitizenCache citizenCache,
            ICompanyCache companyCache,
            IUserCache userCache,
            IRelationshipCache relationshipCache)
        {
            _logger = logger;
            _citizenCache = citizenCache;
            _companyCache = companyCache;
            _userCache = userCache;
            _relationshipCache = relationshipCache;
        }
        #endregion


        [HttpGet("citizen", Name = "Citizen Get")]
        public string Get()
        {
            return "There are " + _citizenCache.FemaleCitizens.Count.ToString() + "female citizens";
        }

        [HttpGet("company/{username}", Name = "Company Get")]
        public string InnerGet(string username)
        {
            if (_userCache.Users.TryGetValue(username, out User user)) return _companyCache.PlayerCompanies[user.CompanyId].Describe();
            else return "That company doesn't exist";
        }
    }
}