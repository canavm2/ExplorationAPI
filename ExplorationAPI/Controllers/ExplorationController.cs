using FileTools;
using Microsoft.AspNetCore.Mvc;

namespace ExplorationAPI.Controllers
{




    [ApiController]
    [Route("[controller]")]
    public class ExplorationController : ControllerBase
    {
        #region Logger
        private readonly ILogger<ExplorationController> _logger;
        public ExplorationController(ILogger<ExplorationController> logger) {_logger = logger;}
        #endregion


        [HttpGet(Name = "Base Get")]
        public string Get(string id)
        {
            return "test";
        }

        [HttpGet("test2", Name = "inner Get")]
        public string InnerGet(string id)
        {
            return "test";
        }
    }
}