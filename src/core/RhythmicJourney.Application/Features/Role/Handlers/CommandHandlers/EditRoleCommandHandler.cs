using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Commands;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Role.Handlers.CommandHandlers;

/// <summary>
/// Rol uzerinde duzeliw etme sorgusunu reallawdiran handlerdir.
/// </summary>
public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand, RoleResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public EditRoleCommandHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<RoleResult> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        bool isRoleExists = await _unitOfWork.RoleRepository.IsRoleExistsAsync(request.roleIdentityDTO.RoleID);
        {
            if (isRoleExists)
            {
                AppRole existingRole = _unitOfWork.RoleRepository.GetRoleById(request.roleIdentityDTO.RoleID);
                {
                    existingRole.Name = request.DTO.newRoleName;
                }

                {
                    _unitOfWork.RoleRepository.Edit(existingRole);
                    int affectedRowCount = await _unitOfWork.SaveChangesToDB_IdentityDb();

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