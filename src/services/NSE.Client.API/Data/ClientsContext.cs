using Microsoft.EntityFrameworkCore;
using NSE.Client.API.Models;
using NSE.Core.Data;

namespace NSE.Client.API.Data
{
    public class ClientsContext : DbContext, IUnitOfWork
    {
        public ClientsContext(DbContextOptions<ClientsContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Clients> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // WHERE THERE'S RELATIONSHIP, TURNOFF CASCATE DELETE
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientsContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;
            return success;
        }
    }
}
