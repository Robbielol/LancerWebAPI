using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace LancerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;

        }

        [HttpGet("/api/testdata")]
        public IActionResult Get() { return StatusCode(StatusCodes.Status200OK); }

    }
}