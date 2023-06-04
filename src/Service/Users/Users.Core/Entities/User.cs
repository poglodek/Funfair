using Microsoft.AspNetCore.Identity;
using Users.Core.ValueObjects;

namespace Users.Core.Entities;

public class User
{
    public int Id { get;  }
    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public Date DateOfBirth { get; private set; }
    public Date CreatedAt { get; private set; }
    public EmailAddress Email { get; private set; }
    public Password Password { get; private set; }
    public Role Role { get; private set; }

    private User()
    {
        
    }

    public User(Name firstName, Name lastName, Date dateOfBirth, Date createdAt, EmailAddress email, Password password, Role role)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        CreatedAt = createdAt;
        Email = email;
        Password = password;
        Role = role;
    }

    public void SetPassword(IPasswordHasher<User> hasher, string password)
    {
        var hashedPassword =  hasher.HashPassword(this, password);
        Password = new Password(hashedPassword);
    }
}