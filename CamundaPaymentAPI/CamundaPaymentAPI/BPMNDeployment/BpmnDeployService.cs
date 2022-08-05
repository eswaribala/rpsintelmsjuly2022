namespace CamundaPaymentAPI.BPMNDeployment
{
    public class BpmnDeployService : IHostedService
    {
        private readonly BpmnService bpmnService;
        public BpmnDeployService(BpmnService bpmnService)
        {
            this.bpmnService = bpmnService;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await bpmnService.DeployProcessDefinition();

            await bpmnService.CleanupProcessInstances();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
