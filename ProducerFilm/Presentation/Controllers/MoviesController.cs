using Microsoft.AspNetCore.Mvc;
using ProducerFilm.Application.DTOs;
using ProducerFilm.Application.Interfaces;

namespace ProducerFilm.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieListHistoryService _movieService;
    private readonly ILogger<MoviesController> _logger;

    public MoviesController(IMovieListHistoryService movieService, ILogger<MoviesController> logger)
    {
        _movieService = movieService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todos os filmes do histórico
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MovieListHistoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await _movieService.GetAllMoviesAsync();
        return Ok(movies);
    }

    /// <summary>
    /// Obtém apenas os filmes vencedores
    /// </summary>
    [HttpGet("winners")]
    [ProducesResponseType(typeof(IEnumerable<MovieListHistoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWinners()
    {
        var winners = await _movieService.GetWinnersAsync();
        return Ok(winners);
    }

    /// <summary>
    /// Obtém o intervalo de prêmios dos produtores (menor e maior intervalo entre vitórias consecutivas)
    /// </summary>
    [HttpGet("winner-interval")]
    [ProducesResponseType(typeof(WinnerIntervalResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWinnerInterval()
    {
        var intervals = await _movieService.GetWinnerIntervalsAsync();
        return Ok(intervals);
    }

}
