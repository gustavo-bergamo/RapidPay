using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RapidPay.Domain.Authentication.Models;
using RapidPay.Domain.Payment.Models;
using RapidPay.Domain.Store.Models;

namespace RapidPay.Domain.Data;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }
    DbSet<ApplicationUser> Users { get; set; }
    DbSet<CreditCard> CreditCards { get; set; }
    DbSet<Purchase> Purchases { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Fee> Fees { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
    EntityEntry Update(object entity);
}
