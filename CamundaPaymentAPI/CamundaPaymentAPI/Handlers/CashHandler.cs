using Camunda.Worker;

namespace CamundaPaymentAPI.Handlers
{
    [HandlerTopics("cash")]
    public class CashHandler : IExternalTaskHandler
    {
        private ILogger<CashHandler> _logger;

        public CashHandler(ILogger<CashHandler> logger)
        {
            _logger = logger;
        }
        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            //read the values from camunda process

            long OrderAmount = Convert.ToInt32 (externalTask.Variables["orderAmount"].Value.ToString());

            await Task.Delay(1000);
            this._logger.LogInformation($"Amount Paid: {OrderAmount}");

            return new CompleteResult();
        }
    }
}
