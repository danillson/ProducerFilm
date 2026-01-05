using ProducerFilm.Application.DTOs;

namespace ProducerFilm.Application.Interfaces;

public interface IMovieListHistoryService
{
    Task<IEnumerable<MovieListHistoryDto>> GetAllMoviesAsync();
    Task<IEnumerable<MovieListHistoryDto>> GetWinnersAsync();
    Task<WinnerIntervalResponseDto> GetWinnerIntervalsAsync();

}
