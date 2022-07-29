using CatalogAPI.Contexts;
using CatalogAPI.Models;
using CatalogAPI.Repositories;
using CatalogAPI.Schemas;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.ConfigServer;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfigServer();
ConfigurationManager configuration = builder.Configuration;

Dictionary<String, Object> data = new VaultConfiguration(configuration)
    .GetDBCredentials().Result;
Console.WriteLine(data);
SqlConnectionStringBuilder providerCs = new SqlConnectionStringBuilder();
//reading from Vault server
providerCs.InitialCatalog = data["dbname3"].ToString();
providerCs.UserID = data["username"].ToString();
providerCs.Password = data["password"].ToString();
//providerCs.DataSource = "DESKTOP-55AGI0I\\MSSQLEXPRESS2021";
//reading via config server
providerCs.DataSource = configuration["servername"];

//providerCs.UserID = CryptoService2.Decrypt(ConfigurationManager.
//AppSettings["UserId"]);
providerCs.MultipleActiveResultSets = true;
providerCs.TrustServerCertificate = false;

builder.Services.AddDbContext<CatalogContext>(o =>
o.UseSqlServer(providerCs.ToString()));


builder.Services.AddDbContext<CatalogIdentityContext>(options =>
options.UseSqlServer(configuration.
GetConnectionString("Identity_Conn_String")));
// Add services to the container.
//DI singelton,scoped or transient
builder.Services.AddScoped<ICatalogRepo, CatalogRepo>();
builder.Services.AddControllers();
builder.Services.AddDiscoveryClient(configuration);
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

builder.Services.AddSwaggerGen();

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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
});
app.UseGraphQL<CatalogSchema>();
app.UseGraphQLPlayground(options: new PlaygroundOptions());

app.Run();
