using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Commands;
using Kateqoriya = RhythmicJourney.Core.Entities.Music.Category;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

namespace RhythmicJourney.Application.Features.Category.Handlers.CommandHandlers;

/// <summary>
/// Kateqoriya yaratma sorgusunu reallawdiran handlerdir.
/// </summary>
public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryResult>
{
    private readonly ICategoryRepository _categoryRepository;
    public AddCategoryCommandHandler(ICategoryRepository categoryRepository) => this._categoryRepository = categoryRepository;

    public async Task<CategoryResult> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        Kateqoriya category = new Kateqoriya()
        {
            Name = request.DTO.CategoryName
        };

        {
            int affectedRowCount = _categoryRepository.Add(category);

            return await CategoryResult.SuccessAsync($"Added {affectedRowCount} data.");
        }
    }
}