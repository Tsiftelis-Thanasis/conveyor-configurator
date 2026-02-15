using Microsoft.EntityFrameworkCore;
using ConveyorApi.Models;

namespace ConveyorApi.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<ProfileSeries> ProfileSeries => Set<ProfileSeries>();
    public DbSet<TrackProfile> TrackProfiles => Set<TrackProfile>();
    public DbSet<TrackBend> TrackBends => Set<TrackBend>();
    public DbSet<Bracket> Brackets => Set<Bracket>();
    public DbSet<Trolley> Trolleys => Set<Trolley>();
    public DbSet<FlightBar> FlightBars => Set<FlightBar>();
    public DbSet<Switch> Switches => Set<Switch>();
    public DbSet<Stopper> Stoppers => Set<Stopper>();
    public DbSet<SwivelUnit> SwivelUnits => Set<SwivelUnit>();
    public DbSet<BridgeInterlock> BridgeInterlocks => Set<BridgeInterlock>();
    public DbSet<DropLiftUnit> DropLiftUnits => Set<DropLiftUnit>();
    public DbSet<Accessory> Accessories => Set<Accessory>();
    public DbSet<BearingOption> BearingOptions => Set<BearingOption>();
    public DbSet<TurnTableSwitch> TurnTableSwitches => Set<TurnTableSwitch>();
    public DbSet<PneumaticControl> PneumaticControls => Set<PneumaticControl>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Category - unique code per client
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.Code }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
        });

        // Material - unique code per client
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.Code }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
        });

        // ProfileSeries - unique series code per client
        modelBuilder.Entity<ProfileSeries>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.SeriesCode }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
        });

        // TrackProfile
        modelBuilder.Entity<TrackProfile>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
        });

        // TrackBend
        modelBuilder.Entity<TrackBend>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // Bracket
        modelBuilder.Entity<Bracket>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // Trolley
        modelBuilder.Entity<Trolley>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // FlightBar
        modelBuilder.Entity<FlightBar>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // Switch
        modelBuilder.Entity<Switch>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // Stopper
        modelBuilder.Entity<Stopper>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // SwivelUnit
        modelBuilder.Entity<SwivelUnit>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // BridgeInterlock
        modelBuilder.Entity<BridgeInterlock>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // DropLiftUnit
        modelBuilder.Entity<DropLiftUnit>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // Accessory
        modelBuilder.Entity<Accessory>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // BearingOption
        modelBuilder.Entity<BearingOption>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
        });

        // TurnTableSwitch
        modelBuilder.Entity<TurnTableSwitch>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Series).WithMany().HasForeignKey(e => e.SeriesId);
            entity.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });

        // PneumaticControl
        modelBuilder.Entity<PneumaticControl>(entity =>
        {
            entity.HasIndex(e => new { e.ClientId, e.PartNumber }).IsUnique();
            entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
            entity.HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);
        });
    }
}
