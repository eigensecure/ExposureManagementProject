using Blazored.LocalStorage;
using CloudAccountsUI;
using CloudAccountsUI.ServiceConfig;
using CloudAccountsUI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var apiBaseUrl = builder.Configuration["ServerURL:ActiveUrl"];
var activeBaseUrl = builder.Configuration[$"ServerURL:{apiBaseUrl}"];

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTransient<JwtTokenHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(activeBaseUrl!);
})
.AddHttpMessageHandler<JwtTokenHandler>();



builder.Services.AddDIServices();

await builder.Build().RunAsync();