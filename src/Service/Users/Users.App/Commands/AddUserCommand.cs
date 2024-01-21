using Funfair.Shared.App.Command;
using Funfair.Shared.Core.Events;
using MediatR;
using Microsoft.Azure.Cosmos;

namespace Users.App.Commands;

public class AddUserCommand : IRequestTransactionalCommand<Unit>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }

    public AddUserCommand(string firstName, string lastName, DateTime dateOfBirth, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        Password = password;    
    }

    public TransactionalBatch TransactionalBatch { get; set; }
    
}