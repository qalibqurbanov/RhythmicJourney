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
/// Kateqoriyani redakte etme sorgusunu reallawdiran handler.
/// </summary>
public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, CategoryResult>
{
    private readonly ICategoryRepository _categoryRepository;
    public EditCategoryCommandHandler(ICategoryRepository categoryRepository) => this._categoryRepository = categoryRepository;

    public async Task<CategoryResult> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        Kateqoriya category = _categoryRepository.GetCategories(cat => cat.Id == request.categoryIdentityDTO.CategoryID).FirstOrDefault();
        {
            if (category == null)
            {
                return await CategoryResult.FailureAsync("Category not exists.");
            }
            else
            {
                {
                    category.Name = request.DTO.NewCategoryName;
                }

                {
                    int affectedRowCount = _categoryRepository.Edit(category);

                    return await CategoryResult.SuccessAsync($"Updated {affectedRowCount} data.");
                }
            }
        }
    }
}