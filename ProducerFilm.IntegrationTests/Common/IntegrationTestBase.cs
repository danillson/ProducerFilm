using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProducerFilm.Domain.Entities;
using ProducerFilm.Infrastructure.Data;
using ProducerFilm.IntegrationTests.Factories;

namespace ProducerFilm.IntegrationTests.Common;

public class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;

    public IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    protected async Task SeedDatabaseAsync(Action<AppDbContext> seedAction)
    {
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Limpar banco antes de popular
        context.MovieListHistories.RemoveRange(context.MovieListHistories);
        await context.SaveChangesAsync();
        
        // Executar ação de seed
        seedAction(context);
        await context.SaveChangesAsync();
    }

    protected async Task<List<MovieListHistory>> GetAllMoviesFromDatabaseAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.MovieListHistories.ToListAsync();
    }
}
