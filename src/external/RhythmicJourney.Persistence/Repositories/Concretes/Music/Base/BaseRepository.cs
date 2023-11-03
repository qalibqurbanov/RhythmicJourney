using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music.Base;

namespace RhythmicJourney.Persistence.Repositories.Concretes.Music.Base;

/// <summary>
/// Umumi funksionalliqlarin implementasiyalarini saxlayir.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly RhythmicJourneyStandartDbContext _dbContext;
    public BaseRepository(RhythmicJourneyStandartDbContext dbContext) => this._dbContext = dbContext;

    private DbSet<TEntity> Table_TEntity => _dbContext.Set<TEntity>();

    #region IBaseRepository
    public int Add(TEntity entity)
    {
        this.Table_TEntity.Add(entity);

        return this._dbContext.SaveChanges();
    }

    public int Edit(TEntity entity, Action<EntityEntry<TEntity>> rules = null)
    {
        if (entity == null)
        {
            throw new ArgumentNullException();
        }

        var entry = this.Table_TEntity.Entry(entity);

        if (rules != null)
        {
            foreach (var item in typeof(TEntity).GetProperties().Where(prop => prop.CanWrite))
            {
                entry.Property(item.Name).IsModified = false;
            }

            rules(entry);
        }

        entry.State = EntityState.Modified;

        return this._dbContext.SaveChanges();
    }

    public int Remove(TEntity entity)
    {
        this.Table_TEntity.Remove(entity);

        return this._dbContext.SaveChanges();
    }
    #endregion IBaseRepository
}