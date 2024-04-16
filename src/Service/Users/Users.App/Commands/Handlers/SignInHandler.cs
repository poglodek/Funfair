using Funfair.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Users.App.Exceptions;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Users.App.Commands.Handlers;

public class SignInHandler : IRequestHandler<SignInCommand,JwtTokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IJsonWebTokenManager _tokenManager;

    public SignInHandler(IUserRepository userRepository,IPasswordHasher<User> hasher, IJsonWebTokenManager tokenManager)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _tokenManager = tokenManager;
    }
    
    public async Task<JwtTokenDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.SignIn(request.Email,request.Password,cancellationToken);

        await Task.Delay(Random.Shared.Next(500,1000), cancellationToken).WaitAsync(cancellationToken);
        
        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        return _tokenManager.CreateToken(user.Id, user.Email.Value, user.Role.Name, user.Role.Claims);
    }
}