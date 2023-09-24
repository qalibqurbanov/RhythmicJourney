using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RhythmicJourney.Core.Exceptions;
using RhythmicJourney.Persistence.Repository.Abstract;
using RhythmicJourney.Application.Authentication.Abstract;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Queries;

namespace RhythmicJourney.Application.Features.Identity.Handlers.QueryHandlers;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var userFromDb = await _userRepository.GetUserByEmailAsync(request.Email);

        if (userFromDb is null)
            throw new UserNotFoundException();

        var token = _jwtTokenGenerator.GenerateToken(userFromDb);

        var result = await _userRepository.SignIn(request.Email, request.Password);

        return await AuthenticationResult.Success(userFromDb, token);
    }
}