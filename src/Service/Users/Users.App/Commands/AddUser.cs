using MediatR;

namespace Users.App.Commands;

public class AddUser : IRequest<Unit>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }

    public AddUser(string firstName, string lastName, DateTime dateOfBirth, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        Password = password;    
    }
}