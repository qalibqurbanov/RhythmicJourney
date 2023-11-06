using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Queries;
using Kateqoriya = RhythmicJourney.Core.Entities.Music.Category;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

namespace RhythmicJourney.Application.Features.Category.Handlers.QueryHandlers;

/// <summary>
/// Kateqoriyalari elde etme sorgusunu reallawdiran handler.
/// </summary>
public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CategoryResult>
{
    private readonly ICategoryRepository _categoryRepository;
    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository) => this._categoryRepository = categoryRepository;

    public async Task<CategoryResult> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        List<Kateqoriya> categories = _categoryRepository.GetCategories().ToList();
        {
            if(categories.Count == 0)
            {
                return await CategoryResult.FailureAsync("No categories found.");
            }
            else
            {
                return await CategoryResult.SuccessAsync(categories);
            }
        }
    }
}