using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Commands;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Role.Handlers.CommandHandlers;

/// <summary>
/// Useri rola elave etme sorgusunu reallawdiran handlerdir.
/// </summary>
public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand, RoleResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public AddUserToRoleCommandHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<RoleResult> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    {
        if(await _unitOfWork.UserRepository.IsUserExistsAsync(request.DTO.UserID))
        {
            if(await _unitOfWork.RoleRepository.IsRoleExistsAsync(request.DTO.RoleID))
            {
                IdentityResult result = await _unitOfWork.RoleRepository.AddUserToRoleAsync(request.DTO.UserID, request.DTO.RoleID);

                if(!result.Succeeded)
                {
                    List<string> errors = result.Errors.Select(error => error.Description).ToList();

                    return await RoleResult.FailureAsync(errors);
                }
                else
                {
                    return await RoleResult.SuccessAsync($"User with id {request.DTO.UserID} successfully added to role with id {request.DTO.RoleID}.");
                }
            }
            else
            {
                return await RoleResult.FailureAsync($"Role with ID {request.DTO.RoleID} not found!");
            }
        }
        else
        {
            return await RoleResult.FailureAsync($"User with ID {request.DTO.UserID} not found!");
        }
    }
}