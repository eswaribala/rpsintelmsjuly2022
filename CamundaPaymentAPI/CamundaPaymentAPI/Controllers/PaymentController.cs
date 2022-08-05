using Camunda.Api.Client;
using Camunda.Api.Client.ProcessDefinition;
using CamundaPaymentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CamundaPaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private ILogger<PaymentController> _logger;
        private IConfiguration _configuration;
        private CamundaClient _client;
        public PaymentController(ILogger<PaymentController> logger,
            IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
            _client = CamundaClient.Create(this._configuration["RestApiUri"]);

        }

        [HttpPost("startProcess")]
        public IActionResult StartProcess([FromBody] Order Order, PaymentBPMNProcess PaymentBPMNProcess)
        {
            _logger.LogInformation("Starting the sample Camunda process...");
            try
            {
                Random random = new Random();

                //Creating process parameters
                StartProcessInstance processParams;

                //json to string
                // String message = JsonConvert.SerializeObject(userAccount);
                //string to c# pobject
                //  UserAccount userAccountObj = JsonConvert.DeserializeObject<UserAccount>(message);



                processParams = new StartProcessInstance()
                    .SetVariable("OrderId", "")
                   .SetVariable("CustomerId", "")
                   .SetVariable("OrderDate","")
                   .SetVariable("OrderAmount", "")


                ;


                _logger.LogInformation($"Camunda process to demonstrate Saga based orchestrator started..........");


                //Startinng the process
                var proceStartResult = _client.ProcessDefinitions.ByKey(PaymentBPMNProcess.ToString())
                    .StartProcessInstance(processParams);

                //return Ok("Done");

                return Ok(proceStartResult.Result.DefinitionId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"error occured!! error messge: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("processDefinitions")]
        //camunda rest api calls
        public async Task<IActionResult> GetProcessDefinitions()
        {



            using var client = new HttpClient();

            var url = this._configuration["RestApiUri"] + "process-definition";
            //synchronous call
            var result = await client.GetAsync(url);
            return Ok(result.Content.ReadAsStringAsync());
            //return Ok(this._client.ProcessDefinitions);
        }


    }
    public enum PaymentBPMNProcess
    {
        Process_Payment
    }
}