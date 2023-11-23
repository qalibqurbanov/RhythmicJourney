using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Queries;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Role.Handlers.QueryHandlers;

/// <summary>
/// Rollari elde etme sorgusunu reallawdiran handlerdir.
/// </summary>
public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, RoleResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetRolesQueryHandler(IUnitOfWork unitOfWork) => this._unitOfWork = unitOfWork;

    public async Task<RoleResult> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        List<AppRole> roles = _unitOfWork.RoleRepository.GetRoles().ToList();
        {
            if (roles.Count == 0)
            {
                return await RoleResult.SuccessAsync("There is no roles.");
            }
            else
            {
                return await RoleResult.SuccessAsync(roles);
            }
        }
    }
}