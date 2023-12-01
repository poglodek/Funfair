namespace Users.Infrastructure.Dto;

public record UserDto
{
    public UserDto()
    {
        
    }

    public UserDto(Guid id, string email, string firstName, string lastName, DateTime dateOfBirth, DateTime createdAt, string role)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        CreatedAt = createdAt;
        Role = role;
    }

    public Guid Id { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Role { get; init; }
    
}