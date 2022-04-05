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
        private readonly FileTool _filetool;
        public ExplorationController(ILogger<ExplorationController> logger, FileTool filetool)
        {
            _logger = logger;
            _filetool = filetool;
        }
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