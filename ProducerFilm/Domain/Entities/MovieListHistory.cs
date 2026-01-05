namespace ProducerFilm.Domain.Entities;

public class MovieListHistory
{
    public int Id { get; private set; }
    public int Year { get; private set; }
    public string Title { get; private set; }
    public string? Studios { get; private set; }
    public string? Producers { get; private set; }
    public string? Winner { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Construtor privado para EF Core
    private MovieListHistory() 
    {
        Title = string.Empty;
    }

    // Construtor para criação da entidade
    public MovieListHistory(int year, string title, string? studios, string? producers, string? winner)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (year < 1900 || year > DateTime.UtcNow.Year + 10)
            throw new ArgumentException("Invalid year", nameof(year));

        Year = year;
        Title = title;
        Studios = studios;
        Producers = producers;
        Winner = winner;
        CreatedAt = DateTime.UtcNow;
    }

    // Método de domínio para verificar se é vencedor
    public bool IsWinner() => !string.IsNullOrWhiteSpace(Winner) && Winner.Equals("yes", StringComparison.OrdinalIgnoreCase);

    // Método de domínio para obter lista de produtores
    public IEnumerable<string> GetProducersList()
    {
        if (string.IsNullOrWhiteSpace(Producers))
            return Enumerable.Empty<string>();

        return Producers
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Replace(" and ", " ").Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct();
    }

    // Método para atualizar informações
    public void Update(string title, string? studios, string? producers, string? winner)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        Title = title;
        Studios = studios;
        Producers = producers;
        Winner = winner;
    }
}
