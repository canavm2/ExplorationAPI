using People;
using Microsoft.AspNetCore.Mvc;

namespace ExplorationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExplorationController : ControllerBase
    {
        #region Logger
        private readonly ILogger<ExplorationController> _logger;
        private ICitizenCache _citizenCache;

        public ExplorationController(ILogger<ExplorationController> logger, ICitizenCache citizenCache)
        {
            _logger = logger;
            _citizenCache = citizenCache;
        }
        #endregion


        [HttpGet(Name = "Base Get")]
        public string Get()
        {
            return "There are " + _citizenCache.FemaleCitizens.Count.ToString() + "female citizens";
        }

        [HttpGet("test2", Name = "inner Get")]
        public string InnerGet()
        {
            return "There are " + _citizenCache.MaleCitizens.Count.ToString() + "male citizens";
        }
    }
}