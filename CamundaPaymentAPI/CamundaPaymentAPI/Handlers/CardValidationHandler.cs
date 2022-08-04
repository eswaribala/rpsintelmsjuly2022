using Camunda.Worker;

namespace CamundaPaymentAPI.Handlers
{
    [HandlerTopics("cardvalidation")]
    public class CardValidationHandler : IExternalTaskHandler
    {
        private ILogger<CardValidationHandler> _logger;

        public CardValidationHandler(ILogger<CardValidationHandler> logger)
        {
            _logger = logger;
        }
        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            //read the values from camunda process

            long CardNo = Convert.ToInt64(externalTask.Variables["cardNo"].Value.ToString());
            string ExpiryDate = externalTask.Variables["expiryDate"].Value.ToString();
            int CVV = Convert.ToInt32(externalTask.Variables["cvv"].Value.ToString());
            await Task.Delay(1000);
            CompleteResult? result = null;
            if (CardNo == 0 || CVV == 0)
            {
                result = new CompleteResult
                {
                    Variables = new Dictionary<String, Variable>
                    {
                        ["valid"] = new Variable("False", VariableType.Boolean)
                    }
                };
            }
            if (CardNo > 0 && CVV > 0)
            {
                result = new CompleteResult
                {
                    Variables = new Dictionary<string, Variable>
                    {
                        ["valid"] = new Variable("True", VariableType.Boolean)
                    }
                };
            }
            return result;
        }
    }
}