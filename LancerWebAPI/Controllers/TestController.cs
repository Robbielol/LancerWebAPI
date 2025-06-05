using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace LancerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private WebsiteModel testModel = new WebsiteModel() 
        {
            Id = Guid.NewGuid(),
            Name = "test",
        };

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;

        }

        [HttpGet("/api/testdata")]
        public IActionResult Get() { return StatusCode(StatusCodes.Status200OK, testModel); }

    }
}