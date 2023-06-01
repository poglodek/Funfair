using Users.Core.Entities;

namespace Users.Core.Repositories;

public interface IRoleRepository
{
    Task<Role> GetDefaultRole();
}