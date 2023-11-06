using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Commands;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

namespace RhythmicJourney.Application.Features.Category.Handlers.CommandHandlers;

/// <summary>
/// Kateqoriyaya musiqi elave etme sorgusunu reallawdiran handler.
/// </summary>
public class AddSongToCategoryCommandHandler : IRequestHandler<AddSongToCategoryCommand, CategoryResult>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISongRepository _songRepository;

    public AddSongToCategoryCommandHandler(ICategoryRepository categoryRepository, ISongRepository songRepository)
    {
        this._categoryRepository = categoryRepository;
        this._songRepository = songRepository;
    }

    public async Task<CategoryResult> Handle(AddSongToCategoryCommand request, CancellationToken cancellationToken)
    {
        {
            if (!_songRepository.IsSongExists(request.DTO.SongID))
            {
                return await CategoryResult.FailureAsync($"Song with ID {request.DTO.SongID} not exists.");
            }

            if (!_categoryRepository.IsCategoryExists(request.DTO.CategoryID))
            {
                return await CategoryResult.FailureAsync($"Category with ID {request.DTO.CategoryID} not exists.");
            }
        }

        {
            int affectedRowCount = _categoryRepository.AddSongToCategory(request.DTO.SongID, request.DTO.CategoryID);

            return await CategoryResult.SuccessAsync($"Updated {affectedRowCount} data.");
        }
    }
}