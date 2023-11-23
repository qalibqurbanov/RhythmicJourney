using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Commands;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Category.Handlers.CommandHandlers;

/// <summary>
/// Kateqoriyaya musiqi elave etme sorgusunu reallawdiran handler.
/// </summary>
public class AddSongToCategoryCommandHandler : IRequestHandler<AddSongToCategoryCommand, CategoryResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public AddSongToCategoryCommandHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<CategoryResult> Handle(AddSongToCategoryCommand request, CancellationToken cancellationToken)
    {
        {
            if (!_unitOfWork.SongRepository.IsSongExists(request.DTO.SongID))
            {
                return await CategoryResult.FailureAsync($"Song with ID {request.DTO.SongID} not exists.");
            }

            if (!_unitOfWork.CategoryRepository.IsCategoryExists(request.DTO.CategoryID))
            {
                return await CategoryResult.FailureAsync($"Category with ID {request.DTO.CategoryID} not exists.");
            }
        }

        {
            _unitOfWork.CategoryRepository.AddSongToCategory(request.DTO.SongID, request.DTO.CategoryID);
            int affectedRowCount = await _unitOfWork.SaveChangesToDB_StandartDb(); 

            return await CategoryResult.SuccessAsync($"Updated {affectedRowCount} data.");
        }
    }
}