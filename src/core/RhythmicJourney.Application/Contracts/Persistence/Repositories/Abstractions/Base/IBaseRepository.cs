using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Base;

/// <summary>
/// Umumi funksionalliqlarin imzalarini saxlayir.
/// </summary>
/// <typeparam name="TEntity">Uzerinde iw goreceyimiz Entity.</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);
    void Edit(TEntity entity, Action<EntityEntry<TEntity>> rules = null);
    void Remove(TEntity entity);
}