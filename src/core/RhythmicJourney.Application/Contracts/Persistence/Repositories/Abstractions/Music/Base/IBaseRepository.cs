using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music.Base;

/// <summary>
/// Umumi funksionalliqlarin imzalarini saxlayir.
/// </summary>
/// <typeparam name="TEntity">Uzerinde iw goreceyimiz Entity.</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    int Add(TEntity entity);
    int Edit(TEntity entity, Action<EntityEntry<TEntity>> rules = null);
    int Remove(TEntity entity);
}