using Funfair.Messaging.EventHubs.Processor;
using Funfair.Shared.App.Events;
using Funfair.Shared.Core.Events;
using MediatR;
using NSubstitute;
using Shouldly;
using Users.App.Commands.Handlers;
using Users.App.Exceptions;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Funfair.App.Tests;

public class AddUser
{
    private Unit Act(Users.App.Commands.AddUserCommand command)
    {
        var addUserHandler = new AddUserHandler(_userRepository,_eventProcessor);
        
        return addUserHandler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();
    }

    [Fact]
    private void AddUser_WithEmailAlreadyExists_ThrowsUserExistsException()
    {
        // Arrange
        var command = ReturnValidUserCommand(_email);

         _userRepository.GetUserByEmail(_email,CancellationToken.None)!.Returns(ReturnValidUserAsync());
        
        // Act
        Record.Exception(()=>Act(command)).ShouldBeOfType<UserExistsException>();
        
    }
    
    [Fact]
    private void AddUser_Valid_ThrowsUserExistsException()
    {
        // Arrange
        var command = ReturnValidUserCommand(_email);

        // Act
        Act(command);
        
        _userRepository.Received(1).AddUser(Arg.Any<User>(),CancellationToken.None);
        

    }

    private Task<User> ReturnValidUserAsync()
    {
        var user = ReturnUser();

        return Task.FromResult(user);
    }

    private User ReturnUser() => User.CreateInstance("John", "Doe", new DateTime(1980, 1, 1), DateTime.Now, _email, "strongPassword123", new Role());

    private readonly string _email = "jack@doe.com";
    
    private  Users.App.Commands.AddUserCommand ReturnValidUserCommand(string mail) =>
        new Users.App.Commands.AddUserCommand("John", "Doe", new DateTime(1980, 1, 1),mail,
            "strongPassword123");


    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IEventDispatcher _eventProcessor = Substitute.For<IEventDispatcher>();

}