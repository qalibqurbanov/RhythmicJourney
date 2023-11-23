using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Queries;
using Kateqoriya = RhythmicJourney.Core.Entities.Music.Category;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Category.Handlers.QueryHandlers;

/// <summary>
/// Kateqoriyalari elde etme sorgusunu reallawdiran handler.
/// </summary>
public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CategoryResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetCategoriesQueryHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<CategoryResult> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        List<Kateqoriya> categories = _unitOfWork.CategoryRepository.GetCategories().ToList();
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