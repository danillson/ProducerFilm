using ProducerFilm.Application.DTOs;
using ProducerFilm.Application.Interfaces;
using ProducerFilm.Domain.Entities;
using ProducerFilm.Domain.Interfaces;
using ProducerFilm.Domain.Services;

namespace ProducerFilm.Application.Services;

public class MovieListHistoryService : IMovieListHistoryService
{
    private readonly IMovieListHistoryRepository _repository;
    private readonly WinnerIntervalDomainService _winnerIntervalService;
    private readonly ILogger<MovieListHistoryService> _logger;

    public MovieListHistoryService(
        IMovieListHistoryRepository repository,
        WinnerIntervalDomainService winnerIntervalService,
        ILogger<MovieListHistoryService> logger)
    {
        _repository = repository;
        _winnerIntervalService = winnerIntervalService;
        _logger = logger;
    }

    public async Task<IEnumerable<MovieListHistoryDto>> GetAllMoviesAsync()
    {
        _logger.LogInformation("Obter todos");
        var movies = await _repository.GetAllAsync();
        return movies.Select(MapToDto);
    }

    public async Task<IEnumerable<MovieListHistoryDto>> GetWinnersAsync()
    {
        _logger.LogInformation("Obter vencedores");
        var winners = await _repository.GetWinnersAsync();
        return winners.Select(MapToDto);
    }

    public async Task<WinnerIntervalResponseDto> GetWinnerIntervalsAsync()
    {
        _logger.LogInformation("Calculando a lista de indicados");
        var winners = await _repository.GetWinnersAsync();
        var result = _winnerIntervalService.CalculateWinnerIntervals(winners);

        return new WinnerIntervalResponseDto
        {
            Min = result.Min.Select(i => new ProducerIntervalDto
            {
                Producer = i.Producer,
                Interval = i.Interval,
                PreviousWin = i.PreviousWin,
                FollowingWin = i.FollowingWin
            }).ToList(),
            Max = result.Max.Select(i => new ProducerIntervalDto
            {
                Producer = i.Producer,
                Interval = i.Interval,
                PreviousWin = i.PreviousWin,
                FollowingWin = i.FollowingWin
            }).ToList()
        };
    }

    private static MovieListHistoryDto MapToDto(MovieListHistory entity)
    {
        return new MovieListHistoryDto
        {
            Id = entity.Id,
            Year = entity.Year,
            Title = entity.Title,
            Studios = entity.Studios,
            Producers = entity.Producers,
            Winner = entity.Winner,
            CreatedAt = entity.CreatedAt
        };
    }
}
