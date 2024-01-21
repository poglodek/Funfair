using Funfair.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Users.Core.Events;
using Users.Core.ValueObjects;

namespace Users.Core.Entities;

public class User : DomainBase
{
    [JsonProperty("firstName")]
    public Name FirstName { get; private set; }
    
    [JsonProperty("lastName")]
    public Name LastName { get; private set; }
    
    [JsonProperty("dateOfBirth")]
    public Date DateOfBirth { get; private set; }
    
    [JsonProperty("createdAt")]
    public Date CreatedAt { get; private set; }
    
    [JsonProperty("email")]
    public EmailAddress Email { get; private set; }
    
    [JsonProperty("password")]
    public Password Password { get; private set; }
    
    [JsonProperty("role")]
    public Role Role { get; private set; }

    private User()
    {
        
    }

    private User(Name firstName, Name lastName, Date dateOfBirth, Date createdAt, EmailAddress email, Password password, Role role)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        CreatedAt = createdAt;
        Email = email;
        Password = password;
        Role = role;
    }

    public static User CreateInstance(Name firstName, Name lastName, Date dateOfBirth, Date createdAt, EmailAddress email, Password password, Role role)
    {
        var user =  new User(firstName, lastName, dateOfBirth, createdAt, email, password, role);
        
        user.RaiseEvent(new SignedUp(user.Id, email.Value, firstName.Value, lastName.Value));
        
        return user;
    }

    public void SetPassword(IPasswordHasher<User> hasher, string password)
    {
        var hashedPassword =  hasher.HashPassword(this, password);
        Password = new Password(hashedPassword);
    }
}