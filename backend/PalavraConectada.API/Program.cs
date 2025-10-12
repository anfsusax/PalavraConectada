// Program.cs - Configuração principal da API
// Como Moisés organizou o tabernáculo, organizamos nossa API
using Microsoft.EntityFrameworkCore;
using PalavraConectada.API.Data;
using PalavraConectada.API.Services;

var builder = WebApplication.CreateBuilder(args);

// ═══════════════════════════════════════════════════════════════════════════
// CONFIGURAÇÃO DE SERVIÇOS
// ═══════════════════════════════════════════════════════════════════════════

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI - Documentação automática
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Palavra Conectada API",
        Version = "v1",
        Description = @"
# 📖 API Palavra Conectada

API inteligente que conecta emoções humanas à Palavra de Deus.

## ✨ Funcionalidades:
- 🧠 **Análise de Emoções:** Detecta sentimentos em texto livre
- 📖 **Busca Inteligente:** Recomenda versículos baseado em emoções
- 🎲 **Versículo Aleatório:** Deixe Deus surpreender
- 📊 **Sistema de Fallback:** Múltiplas fontes de dados

## 🙏 Versículo:
> 'Lâmpada para os meus pés é a tua palavra e luz para o meu caminho.' - Salmos 119:105
        ",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Palavra Conectada",
            Email = "contato@palavraconectada.com"
        }
    });
    
    // Incluir comentários XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Entity Framework + SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=bible.db";

builder.Services.AddDbContext<BibleDbContext>(options =>
    options.UseSqlite(connectionString));

// HttpClient para APIs externas
builder.Services.AddHttpClient();

// Serviços personalizados
builder.Services.AddScoped<EmotionAnalyzerService>();
builder.Services.AddScoped<BibleService>();

// CORS - Permitir requisições do frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",      // Angular
                "https://localhost:5001",     // Blazor
                "http://localhost:5001"       // Blazor HTTP
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Logging
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// ═══════════════════════════════════════════════════════════════════════════
// BUILD DA APLICAÇÃO
// ═══════════════════════════════════════════════════════════════════════════

var app = builder.Build();

// ═══════════════════════════════════════════════════════════════════════════
// INICIALIZAÇÃO DO BANCO DE DADOS
// ═══════════════════════════════════════════════════════════════════════════

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BibleDbContext>();
        
        // Criar banco se não existir
        await context.Database.EnsureCreatedAsync();
        
        app.Logger.LogInformation("✅ Banco de dados inicializado");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "❌ Erro ao inicializar banco de dados");
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// MIDDLEWARE PIPELINE
// ═══════════════════════════════════════════════════════════════════════════

// Swagger (apenas em desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Palavra Conectada API v1");
        options.RoutePrefix = string.Empty; // Swagger na raiz
        options.DocumentTitle = "Palavra Conectada API - Documentação";
    });
    
    app.Logger.LogInformation("📚 Swagger disponível em: https://localhost:7001");
}

app.UseHttpsRedirection();

// CORS - DEVE vir antes de Authorization
app.UseCors("AllowFrontend");

app.UseAuthorization();

// Mapear controllers
app.MapControllers();

// Endpoint de health check
app.MapGet("/health", () => new 
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    version = "1.0.0",
    message = "Palavra Conectada API funcionando! 📖"
})
.WithName("HealthCheck")
.WithOpenApi();

// ═══════════════════════════════════════════════════════════════════════════
// EXECUTAR APLICAÇÃO
// ═══════════════════════════════════════════════════════════════════════════

app.Logger.LogInformation("═══════════════════════════════════════════════════════════");
app.Logger.LogInformation("    📖 PALAVRA CONECTADA API - INICIANDO");
app.Logger.LogInformation("═══════════════════════════════════════════════════════════");
app.Logger.LogInformation("🌐 Swagger: https://localhost:7001");
app.Logger.LogInformation("⚡ API: https://localhost:7001/api");
app.Logger.LogInformation("💚 Health: https://localhost:7001/health");
app.Logger.LogInformation("═══════════════════════════════════════════════════════════");

await app.RunAsync();
