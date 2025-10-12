using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using palavra_conectada_blazor;
using PalavraConectada.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registrar HttpClient - AGORA apontando para NOSSO backend!
builder.Services.AddScoped(sp => 
{
    var client = new HttpClient 
    { 
        BaseAddress = new Uri("http://localhost:7000/") // Usando HTTP para evitar problemas de certificado
    };
    return client;
});

// Registrar o serviço principal - nossa API com IA! 🔥
builder.Services.AddScoped<BackendApiService>();

await builder.Build().RunAsync();
