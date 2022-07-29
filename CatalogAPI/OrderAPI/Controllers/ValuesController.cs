using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ValuesController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return GetValues();
        }

        private string GetValues()
        {
            var httpClient = httpClientFactory.CreateClient("cartApiClient");

            var response = httpClient.GetAsync("WeatherForecast").Result;
            return response.Content.ReadAsStringAsync().Result.ToString();
        }

    }
}
