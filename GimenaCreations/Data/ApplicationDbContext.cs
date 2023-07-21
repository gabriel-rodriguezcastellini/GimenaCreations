using GimenaCreations.Entities;
using GimenaCreations.EntityConfigurations;
using GimenaCreations.Extensions;
using GimenaCreations.MarkerInterfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Security.Claims;

namespace GimenaCreations.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor _contextAccessor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor contextAccessor) : base(options)
    {
        _contextAccessor = contextAccessor;
        ChangeTracker.StateChanged += ChangeTracker_StateChanged;        
    }

    public DbSet<AuditEntry> AuditEntries { get; set; }
    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<PurchaseReception> PurchaseReceptions { get; set; }
    public DbSet<PurchaseReceptionItem> PurchaseReceptionItems { get; set; }
    public DbSet<CatalogType> CatalogTypes { get; set; }
    public DbSet<LoginLogoutAudit> LoginLogoutAudits { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
    public DbSet<PurchaseHistoryItem> PurchaseHistoryItems { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Entities.File> Files { get; set; }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        // Get audit entries
        var auditEntries = OnBeforeSaveChanges();

        // Save current entity
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        // Save audit entries
        await OnAfterSaveChangesAsync(auditEntries);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ApplicationUserEntityTypeConfiguration());
        builder.ApplyConfiguration(new AuditEntryEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new PurchaseReceptionEntityTypeConfiguration());
        builder.ApplyConfiguration(new PurchaseHistoryItemEntityTypeConfiguration());
        builder.ApplyConfiguration(new PurchaseReceptionItemEntityTypeConfiguration());
        builder.ApplyConfiguration(new FileEntityTypeConfiguration());
        builder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        builder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        builder.ApplyConfiguration(new PurchaseEntityTypeConfiguration());
        builder.ApplyConfiguration(new PurchaseItemEntityTypeConfiguration());
        builder.ApplyConfiguration(new SupplierEntityTypeConfiguration());

        builder.Entity<AuditEntry>().Property(ae => ae.Changes)
            .HasConversion(value => JsonConvert.SerializeObject(value), serializedValue => JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedValue));

        base.OnModelCreating(builder);
    }    

    private void ChangeTracker_StateChanged(object sender, EntityStateChangedEventArgs e)
    {
        if (e.Entry.Entity is IUpdateable updateable)
        {
            if (e.Entry.State == EntityState.Modified)
            {
                updateable.ModificationDate = DateTime.Now;

                if (updateable is Purchase purchaseUpdate)
                {
                    PurchaseHistoryItems.Add(new PurchaseHistoryItem
                    {
                        Date = DateTime.UtcNow,
                        PurchaseHistoryId = PurchaseHistories.First(x => x.PurchaseId == purchaseUpdate.Id).Id,
                        State = purchaseUpdate.PurchaseStatus.GetDisplayName()
                    });

                    SaveChanges();
                }
            }
        }
    }

    private List<AuditEntry> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var entries = new List<AuditEntry>();

        foreach (var entry in ChangeTracker.Entries())
        {
            // Dot not audit entities that are not tracked, not changed, or not of type IAuditable
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged || entry.Entity is not IAuditable)
            {
                continue;
            }

            var auditEntry = new AuditEntry
            {
                ActionType = entry.State == EntityState.Added ? "INSERT" : GetActionType(entry),
                EntityId = entry.Properties.Single(p => p.Metadata.IsPrimaryKey()).CurrentValue.ToString(),
                EntityName = entry.Metadata.ClrType.Name,
                ApplicationUserId = _contextAccessor.HttpContext?.User?.Claims?.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? null,
                TimeStamp = DateTime.UtcNow,
                Changes = entry.Properties.Select(p => new { p.Metadata.Name, p.CurrentValue }).ToDictionary(i => i.Name, i => i.CurrentValue),

                // TempProperties are properties that are only generated on save, e.g. ID's
                // These properties will be set correctly after the audited entity has been saved
                TempProperties = entry.Properties.Where(p => p.IsTemporary).ToList(),
            };

            entries.Add(auditEntry);
        }

        return entries;
    }

    private static string GetActionType(EntityEntry entry)
    {
        return entry.State == EntityState.Deleted ? "DELETE" : "UPDATE";
    }

    private Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries)
    {
        if (auditEntries == null || auditEntries.Count == 0)
        {
            return Task.CompletedTask;
        }

        // For each temporary property in each audit entry - update the value in the audit entry to the actual (generated) value
        foreach (var entry in auditEntries)
        {
            foreach (var prop in entry.TempProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    entry.EntityId = prop.CurrentValue.ToString();
                    entry.Changes[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    entry.Changes[prop.Metadata.Name] = prop.CurrentValue;
                }
            }
        }

        AuditEntries.AddRange(auditEntries);
        return SaveChangesAsync();
    }
}