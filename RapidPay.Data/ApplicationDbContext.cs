using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RapidPay.CrossProject.Exceptions;
using RapidPay.Data.Exceptions;
using RapidPay.Data.Infrastructure.Converters;
using RapidPay.Domain.Audit.Models;
using RapidPay.Domain.Authentication.Models;
using RapidPay.Domain.Authentication.Services;
using RapidPay.Domain.Data;
using RapidPay.Domain.Infrastructure.Attributes;
using RapidPay.Domain.Infrastructure.Seed;
using RapidPay.Domain.Payment.Models;
using RapidPay.Domain.Store.Models;
using System.Diagnostics.Contracts;

namespace RapidPay.Data;

[AddDependency(Interface = typeof(IApplicationDbContext))]
public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
    IdentityRole<int>,
    int,
    ApplicationUserClaim,
    IdentityUserRole<int>,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>>, IApplicationDbContext
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApplicationDbContext> _logger;
    private readonly IUserSession _userSession;
    private readonly ISeedServices _seedServices;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IConfiguration configuration,
        ILogger<ApplicationDbContext> logger,
        IUserSession userSession,
        ISeedServices seedServices) : base(options)
    {
        _configuration = configuration;
        _logger = logger;
        _userSession = userSession;
        _seedServices = seedServices;
    }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationUserClaim> UserClaims { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Fee> Fees { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Contract.Requires(builder != null);

        base.OnModelCreating(builder);
        Configure(builder);
    }

    private void Configure(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.HasData(_seedServices.SeedUsers());

            e.HasMany(x => x.CreditCards).WithOne(x => x.ApplicationUser);
            e.HasMany(x => x.UserClaims).WithOne(x => x.ApplicationUser).HasForeignKey(x => x.ApplicationUserId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<ApplicationUserClaim>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.HasData(_seedServices.SeedClaims());
        });

        builder.Entity<CreditCard>(e =>
        {
            e.ToTable(name: nameof(CreditCard), schema: Schemas.Payment);
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.Property(x => x.CreditCardNumber).HasCryptoConversion(_configuration, true);
            e.Property(x => x.CreditCardExpiration).HasCryptoConversion(_configuration, false);

            e.HasMany(x => x.Purchases).WithOne(x => x.CreditCard);
        });

        builder.Entity<Purchase>(e =>
        {
            e.ToTable(name: nameof(Purchase), schema: Schemas.Payment);
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();
        });

        builder.Entity<Product>(e =>
        {
            e.ToTable(name: nameof(Product), schema: Schemas.Payment);
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.HasData(_seedServices.SeedProducts());

            e.HasMany(x => x.Purchases).WithOne(x => x.Product);
        });

        builder.Entity<Fee>(e =>
        {
            e.ToTable(name: nameof(Fee), schema: Schemas.Payment);
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.HasData(_seedServices.SeedFees());
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            PopulateAuditFields(ChangeTracker.Entries());
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            throw new DuplicatedKeyException(_logger, ex);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 547 })
        {
            throw new ForeignKeyException(_logger, ex);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new UserFriendlyException("", "This has already been saved, please reload and try again");
        }
    }

    private void PopulateAuditFields(IEnumerable<EntityEntry> allEntries)
    {
        var entries = allEntries.Where(e => e.Entity.GetType().IsSubclassOf(typeof(AuditEntity)) && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (AuditEntity)entry.Entity;
            EnsurePropertyNotChanged(entry, nameof(entity.CreatedUser));
            EnsurePropertyNotChanged(entry, nameof(entity.CreatedOnUtc));
            EnsurePropertyNotChanged(entry, nameof(entity.ModifiedUser));
            EnsurePropertyNotChanged(entry, nameof(entity.ModifiedOnUtc));
            EnsurePropertyNotChanged(entry, nameof(entity.CreatedIPAddress));
            EnsurePropertyNotChanged(entry, nameof(entity.ModifiedIPAddress));

            var date = DateTime.UtcNow;

            entity.ModifiedOnUtc = date;
            entity.ModifiedUser = _userSession.UserId;
            entity.ModifiedIPAddress = _userSession.IPAddress;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedOnUtc = date;
                entity.CreatedUser = _userSession.UserId;
                entity.CreatedIPAddress = _userSession.IPAddress;
            }
        }
    }

    private void EnsurePropertyNotChanged(EntityEntry entityEntry, string propertyName)
    {
        if (entityEntry.Member(propertyName) is PropertyEntry member)
        {
            if (member.IsModified && !member.CurrentValue.Equals(member.OriginalValue))
            {
                throw new InvalidOperationException($"'{propertyName}' cannot be changed");
            }
        }
    }
}
