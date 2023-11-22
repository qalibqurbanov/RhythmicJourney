using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Commands;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

namespace RhythmicJourney.Application.Features.Role.Handlers.CommandHandlers;

/// <summary>
/// Rolu silme sorgusunu reallawdiran handlerdir.
/// </summary>
public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, RoleResult>
{
    private readonly IRoleRepository _roleRepository;
    public DeleteRoleCommandHandler(IRoleRepository roleRepository) => this._roleRepository = roleRepository;

    public async Task<RoleResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        bool isRoleExists = await _roleRepository.IsRoleExistsAsync(request.DTO.RoleID);
        {
            if (isRoleExists)
            {
                AppRole role = _roleRepository.GetRoleById(request.DTO.RoleID);
                {
                    int affectedRowCount = _roleRepository.Remove(role);

                    return await RoleResult.SuccessAsync($"Deleted {affectedRowCount} data.");
                }
            }
            else
            {
                return await RoleResult.FailureAsync($"Role with ID {request.DTO.RoleID} not found!");
            }
        }
    }
}