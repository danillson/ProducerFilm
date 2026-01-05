using ProducerFilm.Domain.Entities;
using ProducerFilm.Domain.ValueObjects;

namespace ProducerFilm.Domain.Services;

public class WinnerIntervalDomainService
{
    public WinnerIntervalResult CalculateWinnerIntervals(IEnumerable<MovieListHistory> winners)
    {
        if (!winners.Any())
            return new WinnerIntervalResult(Enumerable.Empty<ProducerInterval>(), Enumerable.Empty<ProducerInterval>());

        // Agrupar vitórias por produtor
        var producerWins = new Dictionary<string, List<int>>();

        foreach (var winner in winners.Where(w => w.IsWinner()))
        {
            var producers = winner.GetProducersList();

            foreach (var producer in producers)
            {
                if (!producerWins.ContainsKey(producer))
                    producerWins[producer] = new List<int>();

                if (!producerWins[producer].Contains(winner.Year))
                    producerWins[producer].Add(winner.Year);
            }
        }

        // Calcular intervalos
        var intervals = new List<ProducerInterval>();

        foreach (var producer in producerWins.Where(p => p.Value.Count >= 2))
        {
            var years = producer.Value.OrderBy(y => y).ToList();

            for (int i = 0; i < years.Count - 1; i++)
            {
                var interval = years[i + 1] - years[i];
                intervals.Add(new ProducerInterval(
                    producer.Key,
                    interval,
                    years[i],
                    years[i + 1]
                ));
            }
        }

        if (!intervals.Any())
            return new WinnerIntervalResult(Enumerable.Empty<ProducerInterval>(), Enumerable.Empty<ProducerInterval>());

        // Encontrar menor e maior intervalo
        var minInterval = intervals.Min(i => i.Interval);
        var maxInterval = intervals.Max(i => i.Interval);

        var minIntervals = intervals.Where(i => i.Interval == minInterval);
        var maxIntervals = intervals.Where(i => i.Interval == maxInterval);

        return new WinnerIntervalResult(minIntervals, maxIntervals);
    }
}
