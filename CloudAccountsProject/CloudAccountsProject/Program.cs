using CloudAccounts.Repositories;
using CloudAccountsProject.Data;
using CloudAccountsProject.Repositories;
using CloudAccountsProject.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
 .SetBasePath(Directory.GetCurrentDirectory())
 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
 .Build();

var activeClientURL = config["ClientURL:ActiveURL"];
var clientURL = config[$"ClientURL:{activeClientURL}"];

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CloudAccountsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection")));

builder.Services.AddScoped<ICloudAccountRepository, CloudAccountRepository>();

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
