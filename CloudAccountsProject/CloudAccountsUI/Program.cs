using CloudAccountsUI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

var apiBaseUrl = builder.Configuration["ServerURL:ActiveUrl"];
var activeBaseUrl = builder.Configuration[$"ServerURL:{apiBaseUrl}"];

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(activeBaseUrl!)
});

await builder.Build().RunAsync();