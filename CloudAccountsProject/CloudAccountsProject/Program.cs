using CloudAccountsProject.Repositories;
using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
 .SetBasePath(Directory.GetCurrentDirectory())
 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
 .Build();

var activeClientURL = config["ClientURL:ActiveURL"];
var clientURL = config[$"ClientURL:{activeClientURL}"];

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CloudAccountsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICloudAccountRepository, CloudAccountRepository>();
builder.Services.AddScoped<IBusinessFunctionRepository, BusinessFunctionRepository>();
builder.Services.AddScoped<ICloudHistoryRepository, CloudHistoryRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWasm", policy =>
    {
        policy.WithOrigins(clientURL)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowWasm");

app.MapControllers();

app.Run();
