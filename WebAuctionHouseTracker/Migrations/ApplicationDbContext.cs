using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Migrations;

public class ApplicationDbContext : IdentityDbContext
{

    public DbSet<CommodityInfo> CommodityInfos { get; set; }
    public DbSet<CommodityAuction> CommodityAuctions { get; set; }
    public DbSet<NonCommodityInfo> NonCommodityInfos { get; set; }
    public DbSet<NonCommodityAuction> NonCommodityAuctions { get; set; }

    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<CommodityAuction>()
            .HasKey(x => new{ CommodityId = x.ItemId,x.TimeStamp});

        modelBuilder.Entity<CommodityInfo>()
            .HasKey(x => x.Id);
        // modelBuilder.Entity<CommodityAuction>()
        //     .HasOne(x => x.CommodityInfo)
        //     .WithMany()
        //     .HasForeignKey(x => x.CommodityId);

        modelBuilder.Entity<NonCommodityAuction>()
            .HasKey(x => new{ NonCommodityId = x.ItemId,x.TimeStamp,x.RealmId});

        modelBuilder.Entity<NonCommodityInfo>()
            .HasKey(x => x.Id);
        // modelBuilder.Entity<NonCommodityAuction>()
        //     .HasOne(x => x.NonCommodityInfo)
        //     .WithMany()
        //     .HasForeignKey(x => x.NonCommodityId);
    }
    
    public override int SaveChanges()
    {
        FixEntities(this);
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        FixEntities(this);
        return base.SaveChangesAsync(cancellationToken);
    }

    
    private void FixEntities(ApplicationDbContext context)
    {
        var dateProperties = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime))
            .Select(z => new
            {
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            });

        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity);
        

        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as DateTime?;
                if (originalValue == null)
                    continue;

                prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
            }
        }

    }
}