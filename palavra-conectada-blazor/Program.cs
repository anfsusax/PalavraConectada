using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using palavra_conectada_blazor;
using PalavraConectada.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registrar HttpClient para a API da Bíblia
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://www.abibliadigital.com.br/") });

// Registrar o serviço BibleApiService - como consagrar os levitas para o serviço
builder.Services.AddScoped<BibleApiService>();

await builder.Build().RunAsync();
