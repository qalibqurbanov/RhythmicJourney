using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Persistence.Repositories.Concretes.Base;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

namespace RhythmicJourney.Persistence.Repositories.Concretes.Music;

/// <summary>
/// Kateqoriya ile elaqeli funksionalliqlarin implementasiyalarini saxlayir.
/// </summary>
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    private readonly RhythmicJourneyStandartDbContext _dbContext;
    public CategoryRepository(RhythmicJourneyStandartDbContext dbContext) : base(dbContext) => this._dbContext = dbContext;

    private DbSet<Category> TableCategories => this._dbContext.Set<Category>();
    private DbSet<Song> TableSongs => this._dbContext.Set<Song>();

    #region ICategoryRepository
    public IQueryable<Category> GetCategories(Expression<Func<Category, bool>> expression = null)
    {
        var query = this.TableCategories.AsNoTracking();

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

    public int AddSongToCategory(int songID, int categoryID)
    {
        Song song = TableSongs.FirstOrDefault(song => song.Id == songID);
        {
            song.CategoryId = categoryID;

            int affectedRowCount = _dbContext.SaveChanges();
            return affectedRowCount;
        }
    }

    public bool IsCategoryExists(int categoryID)
    {
        return TableCategories.Where(cat => cat.Id == categoryID).Count() > 0 ? true : false;
    }
    #endregion ICategoryRepository
}