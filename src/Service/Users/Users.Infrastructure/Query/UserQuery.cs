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

public record UserDto(int Id, string Email, string FirstName, string LastName, DateTime DateOfBirth, DateTime CreatedAt, string Role);