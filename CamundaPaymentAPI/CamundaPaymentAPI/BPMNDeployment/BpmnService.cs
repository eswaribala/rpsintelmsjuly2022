using Camunda.Api.Client;
using Camunda.Api.Client.Deployment;
using Camunda.Api.Client.ProcessInstance;

namespace CamundaPaymentAPI.BPMNDeployment
{
    public class BpmnService
    {
        private readonly CamundaClient camunda;
        public BpmnService(String RestApiService)
        {
            camunda = CamundaClient.Create(RestApiService);
        }
        public async Task DeployProcessDefinition()
        {
            var bpmnResourceStream = this.GetType()
                .Assembly
                .GetManifestResourceStream("CamundaPaymentAPI.Processes.PaymentProcess.bpmn");
            

            var bpmnResourceLoginStreamHtml = this.GetType()
                .Assembly
                .GetManifestResourceStream("CamundaPaymentAPI.Forms.LoginForm.html");
            var bpmnResourceOrderStreamHtml = this.GetType()
               .Assembly
               .GetManifestResourceStream("CamundaPaymentAPI.Forms.OrderForm.html");
            var bpmnResourceCardDetailStreamHtml = this.GetType()
             .Assembly
             .GetManifestResourceStream("CamundaPaymentAPI.Forms.CardDetail.html");
            var bpmnResourcePaymentModeStreamHtml = this.GetType()
             .Assembly
             .GetManifestResourceStream("CamundaPaymentAPI.Forms.PaymentMode.html");
           

            try
            {
                await camunda.Deployments.Create(
                    "Payment Process Deployment",
                    true,
                    true,
                    null,
                    null,
                    new ResourceDataContent(bpmnResourceStream, "PaymentProcess.bpmn"),
                    new ResourceDataContent(bpmnResourceLoginStreamHtml, "LoginForm.html"),
                         new ResourceDataContent(bpmnResourceOrderStreamHtml, "OrderForm.html"),
                          new ResourceDataContent(bpmnResourceCardDetailStreamHtml, "CardDetail.html"),
                          new ResourceDataContent(bpmnResourcePaymentModeStreamHtml, "PaymentMode.html")
                             ); 

            }
            catch (Exception e)
            {
                throw new ApplicationException("Failed to deploy process definition", e);
            }
        }

        public async Task CleanupProcessInstances()
        {
            var instances = await camunda.ProcessInstances
                .Query(new ProcessInstanceQuery
                {
                    ProcessDefinitionKey = "Process_"
                })
                .List();

            if (instances.Count > 0)
            {
                await camunda.ProcessInstances.Delete(new DeleteProcessInstances
                {
                    ProcessInstanceIds = instances.Select(i => i.Id).ToList()
                });
            }
        }
    }
}
