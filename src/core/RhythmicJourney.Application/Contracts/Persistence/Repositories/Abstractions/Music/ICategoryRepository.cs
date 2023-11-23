using System;
using System.Linq;
using System.Linq.Expressions;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Base;

namespace RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

/// <summary>
/// Kateqoriya ile elaqeli funksionalliqlarin imzalarini saxlayir.
/// </summary>
public interface ICategoryRepository : IBaseRepository<Category>
{
    IQueryable<Category> GetCategories(Expression<Func<Category, bool>> expression = null);
    IQueryable<Song> GetSongsByCategory(int categoryID);
    void AddSongToCategory(int songID, int categoryID);
    bool IsCategoryExists(int categoryID);
}