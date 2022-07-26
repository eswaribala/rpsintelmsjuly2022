using CatalogAPIDockerCompose.Contexts;
using CatalogAPIDockerCompose.Models;
using CatalogAPIDockerCompose.Repositories;
using CatalogAPIDockerCompose.Schemas;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTracing;
using OpenTracing.Util;
using Prometheus;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//environment
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
        optional: true)
    .Build();
//ConfigurationManager configuration = builder.Configuration;



builder.Services.AddDbContext<CatalogContext>(options =>
options.UseSqlServer(configuration.
GetConnectionString("Catalog_Conn_String")));

builder.Services.AddDbContext<CatalogIdentityContext>(options =>
options.UseSqlServer(configuration.
GetConnectionString("Identity_Conn_String")));
// Add services to the container.
//DI singelton,scoped or transient
builder.Services.AddScoped<ICatalogRepo, CatalogRepo>();
builder.Services.AddControllers();
//ELK
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()

    .WriteTo.Debug()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new
        Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"IntelCatalogIndex-{DateTime.UtcNow:yyyy-MM}"
    })
    .Enrich.WithProperty("Environment", environment)
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
    x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
});
builder.Services.AddScoped<CatalogSchema>();
builder.Services.AddGraphQL()
               .AddSystemTextJson()
               .AddGraphTypes(typeof(CatalogSchema), ServiceLifetime.Scoped);
// For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
   .AddEntityFrameworkStores<CatalogIdentityContext>()
   .AddDefaultTokenProviders();

//Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

//Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
options.RequireHttpsMetadata = false;
options.TokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidAudience = configuration["JWT:ValidAudience"],
    ValidIssuer = configuration["JWT:ValidIssuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
};
});
builder.Services.AddSwaggerGen();
//tracing

builder.Services.AddSingleton<ITracer>(serviceProvider =>
{
    string serviceName = Assembly.GetEntryAssembly().GetName().Name;

    ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    ISampler sampler = new ConstSampler(sample: true);

    ITracer tracer = new Tracer.Builder(serviceName)
        .WithLoggerFactory(loggerFactory)
        .WithSampler(sampler)
        .Build();

    GlobalTracer.Register(tracer);

    return tracer;
});

builder.Services.AddOpenTracing();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMetricServer();
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
});
app.UseGraphQL<CatalogSchema>();
app.UseGraphQLPlayground(options: new PlaygroundOptions());

app.Run();
