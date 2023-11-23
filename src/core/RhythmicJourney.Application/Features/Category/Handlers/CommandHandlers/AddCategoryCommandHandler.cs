using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Commands;
using Kateqoriya = RhythmicJourney.Core.Entities.Music.Category;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Category.Handlers.CommandHandlers;

/// <summary>
/// Kateqoriya yaratma sorgusunu reallawdiran handlerdir.
/// </summary>
public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public AddCategoryCommandHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<CategoryResult> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        Kateqoriya category = new Kateqoriya()
        {
            Name = request.DTO.CategoryName
        };

        {
            _unitOfWork.CategoryRepository.Add(category);
            int affectedRowCount = await _unitOfWork.SaveChangesToDB_StandartDb();

            return await CategoryResult.SuccessAsync($"Added {affectedRowCount} data.");
        }
    }
}