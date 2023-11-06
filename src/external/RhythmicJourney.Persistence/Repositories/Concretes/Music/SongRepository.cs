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
/// Musiqi ile elaqeli funksionalliqlarin implementasiyalarini saxlayir.
/// </summary>
public class SongRepository : BaseRepository<Song>, ISongRepository
{
    private readonly RhythmicJourneyStandartDbContext _dbContext;
    public SongRepository(RhythmicJourneyStandartDbContext dbContext) : base(dbContext) => this._dbContext = dbContext;

    private DbSet<Song> TableSongs => this._dbContext.Set<Song>();

    #region ISongRepository
    public IQueryable<Song> GetSongs(Expression<Func<Song, bool>> expression = null)
    {
        var query = this.TableSongs
            .AsNoTracking()
            .AsQueryable();

        if (expression != null)
        {
            query = query.Where(expression);
        }

        return query.Include(song => song.Category);
    }

    public bool IsSongExists(int songID)
    {
        return TableSongs.Where(song => song.Id == songID).Count() > 0 ? true : false;
    }
    #endregion ISongRepository
}