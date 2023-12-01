using Funfair.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Users.App.Exceptions;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Users.App.Commands.Handlers;

public class SignInHandler : IRequestHandler<SignInCommand,JWTokenDto>
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
    
    public async Task<JWTokenDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email,request.Password);

        if (user is null)
        {
            throw new UserNotFoundException(request.Email);
        }

        return _tokenManager.CreateToken(user.Id.Value, user.Email.Value, user.Role.Name, user.Role.Claims);
    }
}