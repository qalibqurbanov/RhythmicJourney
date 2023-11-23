using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Persistence.Contracts.Repositories.Concretes.Base;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

namespace RhythmicJourney.Persistence.Contracts.Repositories.Concretes.Music;

/// <summary>
/// Kateqoriya ile elaqeli funksionalliqlarin implementasiyalarini saxlayir.
/// </summary>
public class CategoryRepository : BaseRepository<Category, RhythmicJourneyStandartDbContext>, ICategoryRepository
{
    private readonly RhythmicJourneyStandartDbContext _dbContext;
    public CategoryRepository(RhythmicJourneyStandartDbContext dbContext) : base(dbContext) => _dbContext = dbContext;

    private DbSet<Category> TableCategories => _dbContext.Set<Category>();
    private DbSet<Song> TableSongs => _dbContext.Set<Song>();

    #region ICategoryRepository
    public IQueryable<Category> GetCategories(Expression<Func<Category, bool>> expression = null)
    {
        var query = TableCategories.AsNoTracking();

        if (expression != null)
        {
            query = query.Where(expression);
        }

        return query.Include(cat => cat.Songs);
    }

    public IQueryable<Song> GetSongsByCategory(int categoryID)
    {
        var songs = TableCategories
            .AsNoTracking()
            .Where(cat => cat.Id == categoryID)
            .SelectMany(cat => cat.Songs);

        return songs;
    }

    public void AddSongToCategory(int songID, int categoryID)
    {
        Song song = TableSongs.FirstOrDefault(song => song.Id == songID);
        {
            song.CategoryId = categoryID;
        }
    }

    public bool IsCategoryExists(int categoryID)
    {
        return TableCategories.Where(cat => cat.Id == categoryID).Count() > 0 ? true : false;
    }
    #endregion ICategoryRepository
}