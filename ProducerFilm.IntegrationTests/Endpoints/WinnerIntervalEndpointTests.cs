using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ProducerFilm.Application.DTOs;
using ProducerFilm.Domain.Entities;
using ProducerFilm.IntegrationTests.Common;
using ProducerFilm.IntegrationTests.Factories;

namespace ProducerFilm.IntegrationTests.Endpoints;

[Collection("IntegrationTests")]
public class WinnerIntervalEndpointTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public WinnerIntervalEndpointTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    private async Task SeedDatabaseAsync(Action<ProducerFilm.Infrastructure.Data.AppDbContext> seedAction)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProducerFilm.Infrastructure.Data.AppDbContext>();
        
        // Limpar banco antes de popular
        context.MovieListHistories.RemoveRange(context.MovieListHistories);
        await context.SaveChangesAsync();
        
        // Executar ação de seed
        seedAction(context);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetWinnerInterval_ShouldReturnOk_WhenNoWinnersExist()
    {
        // Arrange - Banco vazio

        // Act
        var response = await _client.GetAsync("/api/movies/winner-interval");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<WinnerIntervalResponseDto>();
        result.Should().NotBeNull();
        result!.Min.Should().BeEmpty();
        result.Max.Should().BeEmpty();
    }

    [Fact]
    public async Task GetWinnerInterval_ShouldReturnOk_WhenOnlyOneWinnerExists()
    {
        // Arrange
        await SeedDatabaseAsync(context =>
        {
            context.MovieListHistories.Add(new MovieListHistory(
                1980, 
                "Can't Stop the Music", 
                "Associated Film Distribution", 
                "Allan Carr", 
                "yes"
            ));
        });

        // Act
        var response = await _client.GetAsync("/api/movies/winner-interval");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<WinnerIntervalResponseDto>();
        result.Should().NotBeNull();
        result!.Min.Should().BeEmpty("producer has only one win");
        result.Max.Should().BeEmpty("producer has only one win");
    }

    [Fact]
    public async Task GetWinnerInterval_ShouldCalculateCorrectIntervals_WithSimpleData()
    {
        // Arrange
        await SeedDatabaseAsync(context =>
        {
            // Joel Silver: 1990, 1991 (intervalo de 1 ano)
            context.MovieListHistories.Add(new MovieListHistory(
                1990, 
                "Die Hard 2", 
                "20th Century Fox", 
                "Joel Silver", 
                "yes"
            ));
            context.MovieListHistories.Add(new MovieListHistory(
                1991, 
                "Hudson Hawk", 
                "TriStar Pictures", 
                "Joel Silver", 
                "yes"
            ));

            // Matthew Vaughn: 2002, 2015 (intervalo de 13 anos)
            context.MovieListHistories.Add(new MovieListHistory(
                2002, 
                "Swept Away", 
                "Screen Gems", 
                "Matthew Vaughn", 
                "yes"
            ));
            context.MovieListHistories.Add(new MovieListHistory(
                2015, 
                "Fantastic Four", 
                "20th Century Fox", 
                "Matthew Vaughn", 
                "yes"
            ));
        });

        // Act
        var response = await _client.GetAsync("/api/movies/winner-interval");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<WinnerIntervalResponseDto>();
        result.Should().NotBeNull();

        // Verificar menor intervalo
        result!.Min.Should().HaveCount(1);
        result.Min[0].Producer.Should().Be("Joel Silver");
        result.Min[0].Interval.Should().Be(1);
        result.Min[0].PreviousWin.Should().Be(1990);
        result.Min[0].FollowingWin.Should().Be(1991);

        // Verificar maior intervalo
        result.Max.Should().HaveCount(1);
        result.Max[0].Producer.Should().Be("Matthew Vaughn");
        result.Max[0].Interval.Should().Be(13);
        result.Max[0].PreviousWin.Should().Be(2002);
        result.Max[0].FollowingWin.Should().Be(2015);
    }

    [Fact]
    public async Task GetWinnerInterval_ShouldHandleMultipleProducersWithSameInterval()
    {
        // Arrange
        await SeedDatabaseAsync(context =>
        {
            // Producer A: intervalo de 1 ano
            context.MovieListHistories.Add(new MovieListHistory(
                2000, "Movie A1", "Studio", "Producer A", "yes"));
            context.MovieListHistories.Add(new MovieListHistory(
                2001, "Movie A2", "Studio", "Producer A", "yes"));

            // Producer B: intervalo de 1 ano também
            context.MovieListHistories.Add(new MovieListHistory(
                2010, "Movie B1", "Studio", "Producer B", "yes"));
            context.MovieListHistories.Add(new MovieListHistory(
                2011, "Movie B2", "Studio", "Producer B", "yes"));
        });

        // Act
        var response = await _client.GetAsync("/api/movies/winner-interval");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<WinnerIntervalResponseDto>();
        result.Should().NotBeNull();

        // Ambos devem aparecer no mínimo E máximo (pois são os únicos intervalos)
        result!.Min.Should().HaveCount(2);
        result.Min.Should().Contain(p => p.Producer == "Producer A" && p.Interval == 1);
        result.Min.Should().Contain(p => p.Producer == "Producer B" && p.Interval == 1);

        result.Max.Should().HaveCount(2);
        result.Max.Should().Contain(p => p.Producer == "Producer A" && p.Interval == 1);
        result.Max.Should().Contain(p => p.Producer == "Producer B" && p.Interval == 1);
    }


}
