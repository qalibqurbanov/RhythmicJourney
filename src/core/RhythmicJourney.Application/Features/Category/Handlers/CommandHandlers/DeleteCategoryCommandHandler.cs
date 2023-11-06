using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Commands;
using Kateqoriya = RhythmicJourney.Core.Entities.Music.Category;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

namespace RhythmicJourney.Application.Features.Category.Handlers.CommandHandlers;

/// <summary>
/// Kateqoriyani silme sorgusunu reallawdiran handlerdir.
/// </summary>
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, CategoryResult>
{
    private readonly ICategoryRepository _categoryRepository;
    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository) => this._categoryRepository = categoryRepository;

    public async Task<CategoryResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        if (!_categoryRepository.IsCategoryExists(request.DTO.CategoryID))
        {
            return await CategoryResult.FailureAsync($"Category with ID {request.DTO.CategoryID} not exists.");
        }
        else
        {
            Kateqoriya category = _categoryRepository.GetCategories(cat => cat.Id == request.DTO.CategoryID).FirstOrDefault();

            int affectedRowCount = _categoryRepository.Remove(category);

            return await CategoryResult.SuccessAsync($"Deleted {affectedRowCount} data.");
        }
    }
}