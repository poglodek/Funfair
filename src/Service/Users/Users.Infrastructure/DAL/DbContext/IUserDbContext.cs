using Microsoft.EntityFrameworkCore;
using Users.Core.Entities;

namespace Users.Infrastructure.DAL.DbContext;

public interface IUserDbContext
{
    public DbSet<User> User { get; }
    public Task SaveAsync();
    public void Migration();
    public Task MigrationAsync();
}