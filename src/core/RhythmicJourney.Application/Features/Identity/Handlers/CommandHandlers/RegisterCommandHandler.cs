using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Persistence.Repository.Abstract;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Commands;

namespace RhythmicJourney.Application.Features.Identity.Handlers.CommandHandlers;

/// <summary>
/// Register olma sorgusunu reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    public RegisterCommandHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetUserByEmailAsync(request.Email) is not null)
            return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.DUPLICATE_EMAIL } });

        AppUser user = AppUser.CreateUser(request.FirstName, request.LastName, request.Email, request.Email);
        {
            /*
                * 'email login' ucun hem 'Email' hem de 'UserEmail' fieldlari uygun email adresi("request.Email") saxlamalidir, bu sebeble her ikisine("Email" ve "UserName") eyni datani("request.Email") verirem.
                + Telefonu login kimi iwletmek isteseydik de eyni wekilde hem "UserName" hem de "Telephone" her ikisi telefon nomremize beraber edilmelidir.
            */
        }

        IdentityResult result = await _userRepository.CreateUserAsync(user, request.Password);

        if (result.Succeeded)
            return await AuthenticationResult.SuccessAsync(RhythmicJourney.Core.Constants.IdentityConstants.REGISTER_SUCCESSFUL);
        else
            return await AuthenticationResult.FailureAsync(result.Errors.ToList());
    }
}