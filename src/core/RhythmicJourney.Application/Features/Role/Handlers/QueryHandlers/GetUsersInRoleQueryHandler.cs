using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Queries;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Role.Handlers.QueryHandlers;

/// <summary>
/// X bir roldaki userleri elde etme sorgusunu reallawdiran handlerdir.
/// </summary>
public class GetUsersInRoleQueryHandler : IRequestHandler<GetUsersInRoleQuery, RoleResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetUsersInRoleQueryHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<RoleResult> Handle(GetUsersInRoleQuery request, CancellationToken cancellationToken)
    {
        bool isRoleExists = await _unitOfWork.RoleRepository.IsRoleExistsAsync(request.DTO.RoleID);
        {
            if (isRoleExists)
            {
                AppRole role = _unitOfWork.RoleRepository.GetRoleById(request.DTO.RoleID);
                {
                    List<AppUser> users = await _unitOfWork.RoleRepository.GetUsersInRoleAsync(role.Name);
                    {
                        if (users.Count == 0)
                        {
                            return await RoleResult.SuccessAsync($"No users found with the {role.Name} role.");
                        }
                        else
                        {
                            return await RoleResult.SuccessAsync(users);
                        }
                    }
                }
            }
            else
            {
                return await RoleResult.FailureAsync($"Role with ID {request.DTO.RoleID} not found!");
            }
        }
    }
}