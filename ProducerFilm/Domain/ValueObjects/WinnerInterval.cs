namespace ProducerFilm.Domain.ValueObjects;

public class ProducerInterval
{
    public string Producer { get; }
    public int Interval { get; }
    public int PreviousWin { get; }
    public int FollowingWin { get; }

    public ProducerInterval(string producer, int interval, int previousWin, int followingWin)
    {
        if (string.IsNullOrWhiteSpace(producer))
            throw new ArgumentException("Nao pode ser vazio", nameof(producer));

        if (interval < 0)
            throw new ArgumentException("Nao pode ser negativo", nameof(interval));

        if (previousWin >= followingWin)
            throw new ArgumentException("Verificar período");

        Producer = producer;
        Interval = interval;
        PreviousWin = previousWin;
        FollowingWin = followingWin;
    }
}

public class WinnerIntervalResult
{
    public IReadOnlyList<ProducerInterval> Min { get; }
    public IReadOnlyList<ProducerInterval> Max { get; }

    public WinnerIntervalResult(IEnumerable<ProducerInterval> min, IEnumerable<ProducerInterval> max)
    {
        Min = min?.ToList() ?? new List<ProducerInterval>();
        Max = max?.ToList() ?? new List<ProducerInterval>();
    }
}
