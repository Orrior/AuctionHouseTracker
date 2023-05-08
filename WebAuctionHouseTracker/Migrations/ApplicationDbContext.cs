using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Auctions;

namespace WebApplication1.Migrations;

public class ApplicationDbContext : IdentityDbContext
{

    public DbSet<CommodityInfo> CommodityInfos { get; set; } = default!;
    public DbSet<CommodityAuction> CommodityAuctions { get; set; } = default!;
    public DbSet<NonCommodityInfo> NonCommodityInfos { get; set; } = default!;
    public DbSet<NonCommodityAuction> NonCommodityAuctions { get; set; } = default!;
    
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

        modelBuilder.Entity<NonCommodityAuction>()
            .HasKey(x => new{ NonCommodityId = x.ItemId,x.TimeStamp,x.RealmId});

        modelBuilder.Entity<NonCommodityInfo>()
            .HasKey(x => x.Id);
    }
}