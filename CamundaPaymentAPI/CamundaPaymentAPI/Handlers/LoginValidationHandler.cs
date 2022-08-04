using Camunda.Worker;

namespace CamundaPaymentAPI.Handlers
{
    [HandlerTopics("loginvalidation")]
    public class LoginValidationHandler : IExternalTaskHandler
    {
        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            //read the values from camunda process
          
            string userName = externalTask.Variables["UserName"].Value.ToString();
            string password = externalTask.Variables["Password"].Value.ToString();
            await Task.Delay(1000);
            CompleteResult? result = null;
            if (userName?.Length==0 || password?.Length==0)
            {
                result= new CompleteResult
                {
                    Variables = new Dictionary<string, Variable>
                    {
                        ["status"] = new Variable("False", VariableType.Boolean)
                    }
                };
            }
            if (userName?.Length>0 || password?.Length>0)
            {
                result= new CompleteResult
                {
                    Variables = new Dictionary<string, Variable>
                    {
                        ["status"] = new Variable("True", VariableType.Boolean)
                    }
                };
            }
            return result;
            
        }
    }
}
