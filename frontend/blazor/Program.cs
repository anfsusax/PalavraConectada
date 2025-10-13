using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using palavra_conectada_blazor;
using PalavraConectada.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registrar HttpClient - Detecta automaticamente o ambiente
builder.Services.AddScoped(sp => 
{
    var hostUri = builder.HostEnvironment.BaseAddress;
    
    // Se estiver rodando localmente, usa API local
    var baseAddress = hostUri.Contains("localhost") || hostUri.Contains("127.0.0.1")
        ? new Uri("http://localhost:7000/")
        : new Uri(hostUri); // Em produção, usa o host atual
    
    var client = new HttpClient 
    { 
        BaseAddress = baseAddress
    };
    return client;
});

// Registrar o serviço principal - nossa API com IA! 🔥
builder.Services.AddScoped<BackendApiService>();

await builder.Build().RunAsync();
