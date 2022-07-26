using Polly;
using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDiscoveryClient(configuration);

//Retry Policy
/*
builder.Services.AddHttpClient("cartApiClient", c =>
{
    c.BaseAddress = new Uri("http://localhost:7072/");
}).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
{
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(15),
                 TimeSpan.FromSeconds(15)
            }));
*/

//Circuit Breaker Policy
//circuit opens up after 2 consecutive trials

builder.Services.AddHttpClient("cartApiClient", c => {
    c.BaseAddress =
new Uri("http://localhost:7072");
})
.AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(2, TimeSpan.FromMinutes(2)));


//Bulkhead Policy

builder.Services.AddSingleton<Polly.Bulkhead.AsyncBulkheadPolicy>((x) =>
{
    var policy = Policy.BulkheadAsync(
        maxParallelization: 5,
        maxQueuingActions: 5);

    return policy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
