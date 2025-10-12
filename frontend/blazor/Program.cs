using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using palavra_conectada_blazor;
using PalavraConectada.Services;
using PalavraConectada.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registrar HttpClient - AGORA apontando para NOSSO backend!
builder.Services.AddScoped(sp => 
{
    var client = new HttpClient 
    { 
        BaseAddress = new Uri("https://localhost:7001/") 
    };
    // Permitir certificados self-signed em desenvolvimento
    return client;
});

// Registrar os serviços - como consagrar os levitas para o serviço
builder.Services.AddScoped<BibleApiMockService>(); // Fallback se backend estiver offline
builder.Services.AddScoped<BibleApiService>();      // API externa (fallback)
builder.Services.AddScoped<BackendApiService>();    // 🔥 NOSSO BACKEND (principal)

await builder.Build().RunAsync();
