using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Base;

namespace RhythmicJourney.Persistence.Contracts.Repositories.Concretes.Base;

/// <summary>
/// Umumi funksionalliqlarin implementasiyalarini saxlayir.
/// </summary>
/// <typeparam name="TEntity">Uzerinde iw goreceyimiz Entity.</typeparam>
public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity> /* Gundelik ve Identity Db-larini bir-birinden ayirmiwam deye uzerinde iwleyeceyim DB-ni generic olaraq aliram ki, hemin bu base-i tekrar yaratmayim. */
    where TEntity : class
    where TContext : DbContext
{
    private readonly TContext _dbContext;
    public BaseRepository(TContext dbContext) => this._dbContext = dbContext;

    private DbSet<TEntity> Table_TEntity => _dbContext.Set<TEntity>();

    #region IBaseRepository
    public void Add(TEntity entity)
    {
        Table_TEntity.Add(entity);
    }

    public void Edit(TEntity entity, Action<EntityEntry<TEntity>> rules = null)
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
    }

    public void Remove(TEntity entity)
    {
        Table_TEntity.Remove(entity);
    }
    #endregion IBaseRepository
}