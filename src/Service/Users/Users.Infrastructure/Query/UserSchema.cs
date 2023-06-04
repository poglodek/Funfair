using GraphQL.Types;
using Users.Core.Entities;

namespace Users.Infrastructure.Query;

public sealed class UserSchema : ObjectGraphType<User>
{
    public UserSchema()
    {
        Name = "User";
        Description = "A user";
        
        Field(x => x.Id);
        Field(x => x.FirstName);
        Field(x => x.LastName);
        Field(x => x.DateOfBirth);
        Field(x => x.CreatedAt);
        Field(x => x.Email);
    }
}