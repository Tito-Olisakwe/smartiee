using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartieeWeb;
using SmartieeWeb.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddSingleton<IAppStateService, AppStateService>();


await builder.Build().RunAsync();

