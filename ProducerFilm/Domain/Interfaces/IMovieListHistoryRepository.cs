using ProducerFilm.Domain.Entities;

namespace ProducerFilm.Domain.Interfaces;

public interface IMovieListHistoryRepository
{
    Task<IEnumerable<MovieListHistory>> GetAllAsync();
    Task<MovieListHistory?> GetByIdAsync(int id);
    Task<IEnumerable<MovieListHistory>> GetByYearAsync(int year);
    Task<IEnumerable<MovieListHistory>> GetWinnersAsync();
    Task<MovieListHistory> AddAsync(MovieListHistory movie);
    Task UpdateAsync(MovieListHistory movie);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> SaveChangesAsync();
}
