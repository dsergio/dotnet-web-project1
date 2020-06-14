using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Data
{
    public class ApplicationDbContext : DbContext
    {
#nullable disable
        public DbSet<ModelObject> ModelObjects { get; set; }
       
        private IHttpContextAccessor HttpContextAccessor { get; set; }
#nullable enable

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : base(dbContext) { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        private static void ResetValue(EntityEntry entry, string propertyName)
        {
            PropertyEntry property = entry.Property(propertyName);
            property.CurrentValue = property.OriginalValue;
        }
    }
}
