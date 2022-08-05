using Camunda.Worker;
using Camunda.Worker.Client;
using CamundaPaymentAPI.BPMNDeployment;
using CamundaPaymentAPI.Handlers;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
             options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Camunda worker startUp
builder.Services.AddSingleton(_ => new BpmnService(configuration["RestApiUri"]));
builder.Services.AddHostedService<BpmnDeployService>();
builder.Services.AddExternalTaskClient()
    .ConfigureHttpClient((provider, client) =>
    {
        client.BaseAddress = new Uri(configuration["RestApiUri"]);
    });
builder.Services.AddCamundaWorker("Payment Worker Service", 1)
    .AddHandler<CashHandler>()
    .AddHandler<LoginValidationHandler>()
    .AddHandler<CardValidationHandler>()
    .AddHandler<DeductPayment>();
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Forms")),
    RequestPath = "/Forms",
    EnableDefaultFiles = true
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
