namespace ProducerFilm.Application.DTOs;

public record MovieListHistoryDto
{
    public int Id { get; init; }
    public int Year { get; init; }
    public required string Title { get; init; }
    public string? Studios { get; init; }
    public string? Producers { get; init; }
    public string? Winner { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateMovieListHistoryDto
{
    public required int Year { get; init; }
    public required string Title { get; init; }
    public string? Studios { get; init; }
    public string? Producers { get; init; }
    public string? Winner { get; init; }
}

public record UpdateMovieListHistoryDto
{
    public required string Title { get; init; }
    public string? Studios { get; init; }
    public string? Producers { get; init; }
    public string? Winner { get; init; }
}

public record ProducerIntervalDto
{
    public required string Producer { get; init; }
    public int Interval { get; init; }
    public int PreviousWin { get; init; }
    public int FollowingWin { get; init; }
}

public record WinnerIntervalResponseDto
{
    public List<ProducerIntervalDto> Min { get; init; } = new();
    public List<ProducerIntervalDto> Max { get; init; } = new();
}

public record MovieStatisticsDto
{
    public int TotalMovies { get; init; }
    public int TotalWinners { get; init; }
    public int YearsCount { get; init; }
    public int MinYear { get; init; }
    public int MaxYear { get; init; }
}
