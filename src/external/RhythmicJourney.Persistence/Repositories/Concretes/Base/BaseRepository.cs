using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Base;

namespace RhythmicJourney.Persistence.Repositories.Concretes.Base;

/// <summary>
/// Umumi funksionalliqlarin implementasiyalarini saxlayir.
/// </summary>
/// <typeparam name="TEntity">Uzerinde iw goreceyimiz Entity.</typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly RhythmicJourneyStandartDbContext _dbContext;
    public BaseRepository(RhythmicJourneyStandartDbContext dbContext) => _dbContext = dbContext;

    private DbSet<TEntity> Table_TEntity => _dbContext.Set<TEntity>();

    #region IBaseRepository
    public int Add(TEntity entity)
    {
        Table_TEntity.Add(entity);

        return _dbContext.SaveChanges();
    }

    public int Edit(TEntity entity, Action<EntityEntry<TEntity>> rules = null)
    {
        if (entity == null)
        {
            throw new ArgumentNullException();
        }

        var entry = Table_TEntity.Entry(entity);

        if (rules != null)
        {
            foreach (var item in typeof(TEntity).GetProperties().Where(prop => prop.CanWrite))
            {
                entry.Property(item.Name).IsModified = false;
            }

            rules(entry);
        }

        entry.State = EntityState.Modified;

        return _dbContext.SaveChanges();
    }

    public int Remove(TEntity entity)
    {
        Table_TEntity.Remove(entity);

        return _dbContext.SaveChanges();
    }
    #endregion IBaseRepository
}