using Camunda.Worker;

namespace CamundaPaymentAPI.Handlers
{
    [HandlerTopics("deductpayment")]
    public class DeductPayment : IExternalTaskHandler
    {
        private ILogger<DeductPayment> _logger;

        public DeductPayment(ILogger<DeductPayment> logger)
        {
            _logger = logger;
        }
        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            //read the values from camunda process

            long OrderAmount = Convert.ToInt32(externalTask.Variables["orderAmount"].Value.ToString());

            await Task.Delay(1000);
            this._logger.LogInformation($"Amount Deducted: {OrderAmount}");

            return new CompleteResult();
        }
    }
}

