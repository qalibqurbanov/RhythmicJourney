using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Commands;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

namespace RhythmicJourney.Application.Features.Role.Handlers.CommandHandlers;

/// <summary>
/// Rol uzerinde duzeliw etme sorgusunu reallawdiran handlerdir.
/// </summary>
public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand, RoleResult>
{
    private readonly IRoleRepository _roleRepository;
    public EditRoleCommandHandler(IRoleRepository roleRepository) => this._roleRepository = roleRepository;

    public async Task<RoleResult> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        bool isRoleExists = await _roleRepository.IsRoleExistsAsync(request.roleIdentityDTO.RoleID);
        {
            if (isRoleExists)
            {
                AppRole existingRole = _roleRepository.GetRoleById(request.roleIdentityDTO.RoleID);
                {
                    existingRole.Name = request.DTO.newRoleName;
                }

                {
                    int affectedRowCount = _roleRepository.Edit(existingRole);

                    return await RoleResult.SuccessAsync($"Updated {affectedRowCount} data.");
                }
            }
            else
            {
                return await RoleResult.FailureAsync($"Role with ID {request.roleIdentityDTO.RoleID} not found!");
            }
        }
    }
}