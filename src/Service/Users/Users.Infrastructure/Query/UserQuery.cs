using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.DAL.DbContext;
using Users.Infrastructure.Dto;

namespace Users.Infrastructure.Query;

public sealed class UserQuery
{
    
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<UserDto> GetUsers([Service] UserDbContext user) =>
    user
        .User
        .Select(x=> 
            new UserDto(x.Id.Value, x.Email.Value, x.FirstName.Value, x.LastName.Value, x.DateOfBirth.Value, x.CreatedAt.Value, x.Role.Name));
}

