using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.Services;
using System.Text.Json;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;

        public OrderController(IConfiguration configuration, IOrderService orderService)
        {
            _configuration = configuration;
            _orderService = orderService;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order Order)
        {
            string topicName = this._configuration["TopicName"];
            string message = JsonSerializer.Serialize(Order);
            return Ok(await _orderService.PublishOrder(topicName, message, _configuration));

        }
    }
}
