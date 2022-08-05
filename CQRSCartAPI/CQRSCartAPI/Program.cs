using CQRSCartAPI.Commands;
using CQRSCartAPI.Contexts;
using CQRSCartAPI.Events;
using CQRSCartAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CartContext>
               (options => options.UseSqlite
               (configuration.GetConnectionString("SqliteConnection")));
builder.Services.AddTransient<CartSqliteRepository>();
builder.Services.AddTransient<CartMongoRepository>();
builder.Services.AddTransient<AMQPEventPublisher>();
builder.Services.AddSingleton<CartMessageListener>();
builder.Services.AddScoped<ICommandHandler<Command>, CartCommandHandler>();

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
