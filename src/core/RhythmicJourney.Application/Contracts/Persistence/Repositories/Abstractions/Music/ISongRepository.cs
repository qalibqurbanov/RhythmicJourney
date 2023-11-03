using System;
using System.Linq;
using System.Linq.Expressions;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music.Base;

namespace RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

/// <summary>
/// Musiqi ile elaqeli funksionalliqlarin imzalarini saxlayir.
/// </summary>
public interface ISongRepository : IBaseRepository<Song>
{
    IQueryable<Song> GetSongs(Expression<Func<Song, bool>> expression = null);
}