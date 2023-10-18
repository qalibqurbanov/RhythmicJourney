using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Commands;
using RhythmicJourney.Application.Contracts.Infrastructure.Email.Abstractions;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions;

namespace RhythmicJourney.Application.Features.Identity.Handlers.CommandHandlers;

/// <summary>
/// Register olma sorgusunu reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IEmailSender emailSender,
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        LinkGenerator linkGenerator)
    {
        this._userRepository = userRepository;
        this._emailSender = emailSender;
        this._userManager = userManager;
        this._httpContextAccessor = httpContextAccessor;
        this._linkGenerator = linkGenerator;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetUserByEmailAsync(request.Email) is not null)
            return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.DUPLICATE_EMAIL } });

        AppUser newUser = AppUser.CreateUser(request.FirstName, request.LastName, request.Email, request.Email);
        {
            /*
                * 'email login' ucun hem 'Email' hem de 'UserEmail' fieldlari uygun email adresi("request.Email") saxlamalidir, bu sebeble her ikisine("Email" ve "UserName") eyni datani("request.Email") verirem.
                + Telefonu login kimi iwletmek isteseydik de eyni wekilde hem "UserName" hem de "Telephone" her ikisi telefon nomremize beraber edilmelidir.
            */
        }

        IdentityResult result = await _userRepository.CreateUserAsync(newUser, request.Password);
        {
            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync
                (
                    user: newUser,
                    userManager: _userManager,
                    linkGenerator: _linkGenerator,
                    httpContext: _httpContextAccessor.HttpContext
                );

                return await AuthenticationResult.SuccessAsync(RhythmicJourney.Core.Constants.IdentityConstants.REGISTER_SUCCESSFUL_AND_CONFIRM_EMAIL);

            }
            else
            {
                return await AuthenticationResult.FailureAsync(result.Errors.ToList());
            }
        }
    }
}