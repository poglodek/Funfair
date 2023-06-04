using Microsoft.EntityFrameworkCore;
using Users.Core.Entities;

namespace Users.Infrastructure.DAL.DbContext;

internal class UserDbContext : Microsoft.EntityFrameworkCore.DbContext, IUserDbContext
{
    public DbSet<User> User { get; }
    public Task SaveAsync() => SaveChangesAsync();
    public void Migration() => Database.Migrate();
    public Task MigrationAsync() => Database.MigrateAsync();

    public UserDbContext(DbContextOptions options): base(options)
    {
        
    }
    
}