using Shouldly;
using Users.Core.Entities;
using Users.Core.Exceptions;

namespace Funfair.Core.Tests;

public class RoleTest
{
    [Fact]
    public void CreateRoleAssignDefault_ShouldReturnRole()
    {
        var role = new Role();

        role.Name.ShouldBe(Role.User);
    }
}