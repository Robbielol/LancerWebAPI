using Microsoft.AspNetCore.Mvc;
using LancerWebAPI.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LancerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        IWebsiteServices websiteServices;
        public WebsiteController() 
        {
            websiteServices = new WebsiteServices();
        }

        // GET: api/<WebsiteController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<WebsiteController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<WebsiteController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<WebsiteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WebsiteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("{location}/{query}/{distance}")]
        public async Task<ActionResult<IEnumerable<WebsiteModel>>> GetWebsites(string location, string query,  double distance = 0)
        {

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(query))
            {
                StatusCode(401, "Location and query are needed to supply percise data.");
            }

            List<WebsiteModel> models = (List<WebsiteModel>) await websiteServices.GetAllPlaces(location, query, distance);
            return Ok(models);
        }
    }
}
