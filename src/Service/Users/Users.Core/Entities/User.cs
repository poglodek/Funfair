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
}