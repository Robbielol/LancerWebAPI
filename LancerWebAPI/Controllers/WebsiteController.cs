using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using LancerWebAPI.Services;
using LancerWebAPI.Database;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LancerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteController : ControllerBase
    {
        private IWebsiteServices _websiteServices;
        

        public WebsiteController(WebsiteServices websiteServices) 
        {
            _websiteServices =  websiteServices;
            
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
        // searches
        [HttpGet]
        public async Task<IEnumerable<string>> GetSearches()
        {
            return (IEnumerable<string>) await _websiteServices.GetAllSearchs();
        }

        [HttpGet("{location}/{query}/{distance}")]
        public async Task<ActionResult<IEnumerable<GooglePlaceModel>>> GetWebsites(string location, string query, int distance = 0)
        {

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(query))
            {
                return BadRequest("Location and query are needed to supply percise data.");
            }

            List<GooglePlaceModel> models = (List<GooglePlaceModel>) await _websiteServices.GetAllPlaces(location, query, distance);
            return Ok(models);
        }
    }
}
