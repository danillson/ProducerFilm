using Microsoft.EntityFrameworkCore;
using ProducerFilm.Domain.Entities;
using ProducerFilm.Domain.Interfaces;
using ProducerFilm.Infrastructure.Data;

namespace ProducerFilm.Infrastructure.Repositories;

public class MovieListHistoryRepository : IMovieListHistoryRepository
{
    private readonly AppDbContext _context;

    public MovieListHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovieListHistory>> GetAllAsync()
    {
        return await _context.MovieListHistories
            .OrderByDescending(m => m.Year)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }

    public async Task<MovieListHistory?> GetByIdAsync(int id)
    {
        return await _context.MovieListHistories.FindAsync(id);
    }

    public async Task<IEnumerable<MovieListHistory>> GetByYearAsync(int year)
    {
        return await _context.MovieListHistories
            .Where(m => m.Year == year)
            .OrderBy(m => m.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<MovieListHistory>> GetWinnersAsync()
    {
        return await _context.MovieListHistories
            .Where(m => m.Winner != null && m.Winner.ToLower() == "yes")
            .OrderBy(m => m.Year)
            .ToListAsync();
    }

    public async Task<MovieListHistory> AddAsync(MovieListHistory movie)
    {
        await _context.MovieListHistories.AddAsync(movie);
        return movie;
    }

    public Task UpdateAsync(MovieListHistory movie)
    {
        _context.MovieListHistories.Update(movie);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var movie = await GetByIdAsync(id);
        if (movie != null)
        {
            _context.MovieListHistories.Remove(movie);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.MovieListHistories.AnyAsync(m => m.Id == id);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
