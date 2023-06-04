using Funfair.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Users.App.Exceptions;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Users.App.Commands.Handlers;

public class SignInHandler : IRequestHandler<SignIn,JWTokenDto>
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
    
    public async Task<JWTokenDto> Handle(SignIn request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Mail,request.Password);

        if (user is null)
        {
            throw new UserNotFoundException(request.Mail);
        }

        return _tokenManager.CreateToken(user.Id, user.Email.Value, user.Role.Name, user.Role.Claims);
    }
}