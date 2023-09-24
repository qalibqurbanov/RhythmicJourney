using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Core.Exceptions;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Persistence.Repository.Abstract;
using RhythmicJourney.Application.Authentication.Abstract;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Commands;

namespace RhythmicJourney.Application.Features.Identity.Handlers.CommandHandlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetUserByEmailAsync(request.Email) is not null)
            throw new DuplicateEmailException();

        var user = new AppUser()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email

            /*
                * 'email login' ucun hem 'Email' hem de 'UserEmail' fieldlari uygun email adresi("request.Email") saxlamalidir, bu sebeble her ikisine("Email" ve "UserName") eyni datani("request.Email") verirem.
                + Telefonu login kimi iwletmek isteseydik de eyni wekilde hem "UserName" hem de "Telephone" her ikisi telefon nomremize beraber edilmelidir.
            */
        };

        var token = _jwtTokenGenerator.GenerateToken(user);

        var result = await _userRepository.CreateUserAsync(user, request.Password);

        if (result.Succeeded)
            return await AuthenticationResult.Success(user, token);
        else
            return await AuthenticationResult.Failure(result.Errors.ToList());
    }
}