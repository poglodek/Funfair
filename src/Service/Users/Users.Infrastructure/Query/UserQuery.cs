using HotChocolate;
using HotChocolate.Data;
using Users.Infrastructure.DAL.DbContext;

namespace Users.Infrastructure.Query;

public class UserQuery
{
    
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<UserDto> GetUsers([Service] UserDbContext user) =>
    user.User.Select(x=> new UserDto(x.Id, x.Email.Value, x.FirstName.Value, x.LastName.Value, x.DateOfBirth.Value, x.CreatedAt.Value, x.Role.Name));
}

public class UserDto
{
    public UserDto()
    {
        
    }

    public UserDto(int id, string email, string firstName, string lastName, DateTime dateOfBirth, DateTime createdAt, string role)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        CreatedAt = createdAt;
        Role = role;
    }

    public int Id { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Role { get; init; }
    
}