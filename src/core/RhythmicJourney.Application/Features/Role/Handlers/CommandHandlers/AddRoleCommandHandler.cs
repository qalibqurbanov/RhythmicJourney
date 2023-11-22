using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Commands;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

namespace RhythmicJourney.Application.Features.Role.Handlers.CommandHandlers;

/// <summary>
/// Yeni rol yaratma sorgusunu reallawdiran handlerdir.
/// </summary>
public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, RoleResult>
{
    private readonly IRoleRepository _roleRepository;
    public AddRoleCommandHandler(IRoleRepository roleRepository) => this._roleRepository = roleRepository;

    public async Task<RoleResult> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        bool isRoleExists = await _roleRepository.IsRoleExistsAsync(request.DTO.RoleName);
        {
            if (isRoleExists)
            {
                return await RoleResult.FailureAsync($"Role {request.DTO.RoleName} already exists.");
            }
            else
            {
                AppRole newRole = new AppRole()
                {
                    Name = request.DTO.RoleName,
                    NormalizedName = request.DTO.RoleName.ToUpper()
                };

                {
                    int affectedRowCount = _roleRepository.Add(newRole);

                    return await RoleResult.SuccessAsync($"Added {affectedRowCount} data.");
                }
            }
        }
    }
}