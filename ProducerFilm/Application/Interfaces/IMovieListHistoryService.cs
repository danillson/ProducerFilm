using ProducerFilm.Application.DTOs;

namespace ProducerFilm.Application.Interfaces;

public interface IMovieListHistoryService
{
    Task<IEnumerable<MovieListHistoryDto>> GetAllMoviesAsync();
    Task<MovieListHistoryDto?> GetMovieByIdAsync(int id);
    Task<IEnumerable<MovieListHistoryDto>> GetMoviesByYearAsync(int year);
    Task<IEnumerable<MovieListHistoryDto>> GetWinnersAsync();
    Task<MovieStatisticsDto> GetStatisticsAsync();
    Task<WinnerIntervalResponseDto> GetWinnerIntervalsAsync();
    Task<MovieListHistoryDto> CreateMovieAsync(CreateMovieListHistoryDto dto);
    Task UpdateMovieAsync(int id, UpdateMovieListHistoryDto dto);
    Task DeleteMovieAsync(int id);
}
