using Microsoft.EntityFrameworkCore;
using ProducerFilm.Application.Interfaces;
using ProducerFilm.Application.Services;
using ProducerFilm.Domain.Interfaces;
using ProducerFilm.Domain.Services;
using ProducerFilm.Infrastructure.Data;
using ProducerFilm.Infrastructure.Repositories;
using ProducerFilm.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar SQLite Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Repositórios (Infrastructure Layer)
builder.Services.AddScoped<IMovieListHistoryRepository, MovieListHistoryRepository>();

// Registrar Serviços de Domínio (Domain Layer)
builder.Services.AddScoped<WinnerIntervalDomainService>();

// Registrar Serviços de Aplicação (Application Layer)
builder.Services.AddScoped<IMovieListHistoryService, MovieListHistoryService>();

// Registrar Serviços de Infraestrutura
builder.Services.AddScoped<CsvFileProcessorService>();

// Adicionar serviços do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() 
    { 
        Title = "ProducerFilm API - DDD Architecture", 
        Version = "v1",
        Description = "API para análise de filmes vencedores do Golden Raspberry Awards usando Domain-Driven Design"
    });
});

// Adicionar suporte a Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configurar o middleware do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ProducerFilm API v1");
        options.RoutePrefix = string.Empty;
    });
}

// Executar migrações e processar arquivos ao iniciar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    // Aplicar migrações automaticamente
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("? Banco de dados migrado com sucesso (DDD Architecture)");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "? Erro ao migrar o banco de dados");
    }
    
    // Processar arquivos CSV
    try
    {
        var fileProcessor = services.GetRequiredService<CsvFileProcessorService>();
        await fileProcessor.ProcessCsvFilesAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "? Erro ao processar arquivos CSV");
    }
}

// Mapear Controllers
app.MapControllers();

app.Run();

// Tornar a classe Program acessível para testes de integração
public partial class Program { }
