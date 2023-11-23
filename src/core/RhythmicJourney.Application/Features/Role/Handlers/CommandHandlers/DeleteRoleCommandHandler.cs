using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Commands;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Role.Handlers.CommandHandlers;

/// <summary>
/// Rolu silme sorgusunu reallawdiran handlerdir.
/// </summary>
public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, RoleResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<RoleResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        bool isRoleExists = await _unitOfWork.RoleRepository.IsRoleExistsAsync(request.DTO.RoleID);
        {
            if (isRoleExists)
            {
                AppRole role = _unitOfWork.RoleRepository.GetRoleById(request.DTO.RoleID);
                {
                    _unitOfWork.RoleRepository.Remove(role);
                    int affectedRowCount = await _unitOfWork.SaveChangesToDB_IdentityDb();

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