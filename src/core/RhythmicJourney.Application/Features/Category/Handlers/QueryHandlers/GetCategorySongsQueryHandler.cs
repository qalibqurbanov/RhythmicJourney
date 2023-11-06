using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Queries;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

namespace RhythmicJourney.Application.Features.Category.Handlers.QueryHandlers;

/// <summary>
/// Kateqoriyadaki musiqileri elde etme sorgusunu reallawdiran handler.
/// </summary>
public class GetCategorySongsQueryHandler : IRequestHandler<GetCategorySongsQuery, CategoryResult>
{
    private readonly ICategoryRepository _categoryRepository;
    public GetCategorySongsQueryHandler(ICategoryRepository categoryRepository) => this._categoryRepository = categoryRepository;

    public async Task<CategoryResult> Handle(GetCategorySongsQuery request, CancellationToken cancellationToken)
    {
        List<Song> songs = _categoryRepository.GetSongsByCategory(request.DTO.CategoryID).ToList();
        {
            if (songs.Count == 0)
            {
                return await CategoryResult.SuccessAsync("No songs found in this category.");
            }
            else
            {
                return await CategoryResult.SuccessAsync(songs);
            }
        }
    }
}