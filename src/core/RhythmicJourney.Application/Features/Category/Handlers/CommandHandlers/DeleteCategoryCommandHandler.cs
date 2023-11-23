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
/// Kateqoriyani silme sorgusunu reallawdiran handlerdir.
/// </summary>
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, CategoryResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<CategoryResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        if (!_unitOfWork.CategoryRepository.IsCategoryExists(request.DTO.CategoryID))
        {
            return await CategoryResult.FailureAsync($"Category with ID {request.DTO.CategoryID} not exists.");
        }
        else
        {
            Kateqoriya category = _unitOfWork.CategoryRepository.GetCategories(cat => cat.Id == request.DTO.CategoryID).FirstOrDefault();
            {
                _unitOfWork.CategoryRepository.Remove(category);
                int affectedRowCount = await _unitOfWork.SaveChangesToDB_StandartDb();

                return await CategoryResult.SuccessAsync($"Deleted {affectedRowCount} data.");
            }
        }
    }
}