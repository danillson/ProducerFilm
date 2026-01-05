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
    /// Obtém um filme específico por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MovieListHistoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovieById(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        
        if (movie == null)
        {
            _logger.LogWarning("Movie with ID {Id} not found", id);
            return NotFound(new { message = $"Movie with ID {id} not found" });
        }

        return Ok(movie);
    }

    /// <summary>
    /// Obtém filmes por ano
    /// </summary>
    [HttpGet("year/{year}")]
    [ProducesResponseType(typeof(IEnumerable<MovieListHistoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMoviesByYear(int year)
    {
        var movies = await _movieService.GetMoviesByYearAsync(year);
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
    /// Obtém estatísticas dos filmes
    /// </summary>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(MovieStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatistics()
    {
        var statistics = await _movieService.GetStatisticsAsync();
        return Ok(statistics);
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

    /// <summary>
    /// Cria um novo filme no histórico
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MovieListHistoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieListHistoryDto dto)
    {
        try
        {
            var movie = await _movieService.CreateMovieAsync(dto);
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid data for movie creation");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um filme existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMovie(int id, [FromBody] UpdateMovieListHistoryDto dto)
    {
        try
        {
            await _movieService.UpdateMovieAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Movie with ID {Id} not found for update", id);
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid data for movie update");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deleta um filme
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        try
        {
            await _movieService.DeleteMovieAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Movie with ID {Id} not found for deletion", id);
            return NotFound(new { message = ex.Message });
        }
    }
}
