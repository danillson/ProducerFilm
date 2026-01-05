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
        _logger.LogInformation("Getting all movies from service");
        var movies = await _repository.GetAllAsync();
        return movies.Select(MapToDto);
    }

    public async Task<MovieListHistoryDto?> GetMovieByIdAsync(int id)
    {
        _logger.LogInformation("Getting movie with ID: {Id}", id);
        var movie = await _repository.GetByIdAsync(id);
        return movie != null ? MapToDto(movie) : null;
    }

    public async Task<IEnumerable<MovieListHistoryDto>> GetMoviesByYearAsync(int year)
    {
        _logger.LogInformation("Getting movies from year: {Year}", year);
        var movies = await _repository.GetByYearAsync(year);
        return movies.Select(MapToDto);
    }

    public async Task<IEnumerable<MovieListHistoryDto>> GetWinnersAsync()
    {
        _logger.LogInformation("Getting winner movies");
        var winners = await _repository.GetWinnersAsync();
        return winners.Select(MapToDto);
    }

    public async Task<MovieStatisticsDto> GetStatisticsAsync()
    {
        _logger.LogInformation("Calculating movie statistics");
        var allMovies = await _repository.GetAllAsync();
        var winners = await _repository.GetWinnersAsync();
        var years = allMovies.Select(m => m.Year).Distinct().OrderBy(y => y).ToList();

        return new MovieStatisticsDto
        {
            TotalMovies = allMovies.Count(),
            TotalWinners = winners.Count(),
            YearsCount = years.Count,
            MinYear = years.Any() ? years.Min() : 0,
            MaxYear = years.Any() ? years.Max() : 0
        };
    }

    public async Task<WinnerIntervalResponseDto> GetWinnerIntervalsAsync()
    {
        _logger.LogInformation("Calculating winner intervals");
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

    public async Task<MovieListHistoryDto> CreateMovieAsync(CreateMovieListHistoryDto dto)
    {
        _logger.LogInformation("Creating new movie: {Title}", dto.Title);
        
        var movie = new MovieListHistory(dto.Year, dto.Title, dto.Studios, dto.Producers, dto.Winner);
        var created = await _repository.AddAsync(movie);
        await _repository.SaveChangesAsync();

        return MapToDto(created);
    }

    public async Task UpdateMovieAsync(int id, UpdateMovieListHistoryDto dto)
    {
        _logger.LogInformation("Updating movie with ID: {Id}", id);
        
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with ID {id} not found");

        movie.Update(dto.Title, dto.Studios, dto.Producers, dto.Winner);
        await _repository.UpdateAsync(movie);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteMovieAsync(int id)
    {
        _logger.LogInformation("Deleting movie with ID: {Id}", id);
        
        if (!await _repository.ExistsAsync(id))
            throw new KeyNotFoundException($"Movie with ID {id} not found");

        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
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
