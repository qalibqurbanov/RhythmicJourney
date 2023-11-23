using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Commands;
using Kateqoriya = RhythmicJourney.Core.Entities.Music.Category;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Category.Handlers.CommandHandlers;

/// <summary>
/// Kateqoriyani redakte etme sorgusunu reallawdiran handler.
/// </summary>
public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, CategoryResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public EditCategoryCommandHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<CategoryResult> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        Kateqoriya category = _unitOfWork.CategoryRepository.GetCategories(cat => cat.Id == request.categoryIdentityDTO.CategoryID).FirstOrDefault();
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
                    _unitOfWork.CategoryRepository.Edit(category);
                    int affectedRowCount = await _unitOfWork.SaveChangesToDB_StandartDb();

                    return await CategoryResult.SuccessAsync($"Updated {affectedRowCount} data.");
                }
            }
        }
    }
}