using Funfair.Auth;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shouldly;
using Users.App.Commands;
using Users.App.Commands.Handlers;
using Users.App.Exceptions;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Funfair.App.Tests;

public class SignInTest
{
    
    private Task<JWTokenDto> Act(SignIn command)
    {
        var handler = new SignInHandler(_userRepository,_hasher,_tokenManager);
        return handler.Handle(command, CancellationToken.None);
    }
    
    [Fact]
    public async Task SignInTestAsync_AllValid_ReturnToken()
    {
        var mail = "jack@doe.com";
        var password = "SuperFanyPassword";
        var command = new SignIn(mail,password);

        _userRepository.GetUserByEmail(mail, password).Returns(ReturnValidUserAsync());
        _tokenManager.CreateToken(Arg.Any<int>(), mail, Arg.Any<string>(), Arg.Any<IDictionary<string, string>>())
            .Returns(new JWTokenDto
            {
                Role = Role.Default,
                JWT = "testJWT",
                ExpiresInHours = 8
            });
        
        
        var act = await Act(command);

        act.ShouldNotBeNull();
        act.Role.ShouldBe(Role.Default);
        act.UserId.ShouldBe(ReturnUser().Id);
        act.JWT.ShouldBe("testJWT");
        act.ExpiresInHours.ShouldBe(8);
    }
    
    [Fact]
    public async Task SignInTestAsync_UserNotExits_ShouldException()
    {
        var mail = "jack@doe.com";
        var password = "SuperFanyPassword";
        var command = new SignIn(mail,password);

        var ex = await Record.ExceptionAsync(async () => await Act(command));

        ex.ShouldNotBeNull();
        ex.ShouldBeOfType<UserNotFoundException>();

    }
    
    
    
    private Task<User> ReturnValidUserAsync()
    {
        var user = ReturnUser();

        return Task.FromResult(user);
    }
    
    private User ReturnUser() => new User("John", "Doe", new DateTime(1980, 1, 1), DateTime.Now, "jack@doe.com", "SuperFanyPassword", new Role());
    
    
    
    private IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private IPasswordHasher<User> _hasher = Substitute.For<IPasswordHasher<User>>();
    private IJsonWebTokenManager _tokenManager = Substitute.For<IJsonWebTokenManager>();
}