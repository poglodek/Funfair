using Funfair.Messaging.AzureServiceBus.OutInBoxPattern.Models;
using Funfair.Shared.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Funfair.Messaging.AzureServiceBus.OutInBoxPattern;

public class OutboxDbContext : DbContext, IUnitOfWork,IChangeTrack
{
    public DbSet<Inbox> Inboxes { get; set; }
    public DbSet<Outbox> Outboxes { get; set; }
    
    public OutboxDbContext()
    {
        
    }
    
    public OutboxDbContext(DbContextOptions<OutboxDbContext> options): base(options)
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