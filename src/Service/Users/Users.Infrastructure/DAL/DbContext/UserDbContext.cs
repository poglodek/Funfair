using Funfair.Shared.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Users.Core.Entities;

namespace Users.Infrastructure.DAL.DbContext;

public class UserDbContext : Microsoft.EntityFrameworkCore.DbContext, IUnitOfWork,IChangeTrack
{
    public DbSet<User> User { get; private set; }

    public UserDbContext()
    {
        
    }
    
    public UserDbContext(DbContextOptions<UserDbContext> options): base(options)
    {
               
    }
    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //for migration only
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Server=.;Database=funfair-user;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    
}