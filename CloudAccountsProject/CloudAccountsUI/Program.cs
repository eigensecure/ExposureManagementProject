using CloudAccountsUI;
using CloudAccountsUI.ServiceConfig;
using CloudAccountsUI.Services;
using CloudAccountsUI.Services.Contracts;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddScoped<ICloudAccountService, CloudAccountService>();
builder.Services.AddScoped<IBusinessFunctionService, BusinessFunctionService>();
builder.Services.AddScoped<ICloudRecordsService, CloudRecordsService>();
builder.Services.AddScoped<ICrowdGroupMasterService, CrowdGroupMasterService>();

var apiBaseUrl = builder.Configuration["ServerURL:ActiveUrl"];
var activeBaseUrl = builder.Configuration[$"ServerURL:{apiBaseUrl}"];

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(activeBaseUrl!)
});

builder.Services.AddDIServices();

await builder.Build().RunAsync();