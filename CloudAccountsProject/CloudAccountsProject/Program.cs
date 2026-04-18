using CloudAccountsProject.Data;
using CloudAccountsProject.RepoConfig;
using CloudAccountsProject.Repositories;
using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
 .SetBasePath(Directory.GetCurrentDirectory())
 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
 .Build();

var activeClientURL = config["ClientURL:ActiveURL"];
var clientURL = config[$"ClientURL:{activeClientURL}"];
var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<CloudAccountsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection")));

builder.Services.AddDbContext<CloudAccountsDbSPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection")));

builder.Services.AddDIServices();

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

app.UseCors("AllowWasm");

app.UseAuthentication(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
