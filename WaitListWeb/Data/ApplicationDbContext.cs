using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaitListWeb.Models;
using WaitListWeb.Security;

namespace WaitListWeb.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ITenantProvider? _tenant;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ITenantProvider? tenant = null)
        : base(options)
    {
        _tenant = tenant;
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Queue> Queues => Set<Queue>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<QueueEntry> QueueEntries => Set<QueueEntry>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var accountIdClaim = _tenant?.GetAccountId();
        var isAdmin = _tenant?.IsSystemAdmin() == true;

        var hasAccountId = int.TryParse(accountIdClaim, out var accountId);

        builder.Entity<Queue>().HasQueryFilter(q =>
            isAdmin || (hasAccountId && q.AccountId == accountId));

        builder.Entity<Customer>().HasQueryFilter(c =>
            isAdmin || (hasAccountId && c.AccountId == accountId));

        builder.Entity<Service>().HasQueryFilter(s =>
            isAdmin || (hasAccountId && s.AccountId == accountId));

        builder.Entity<QueueEntry>().HasQueryFilter(e =>
            isAdmin || (hasAccountId && e.AccountId == accountId));
    }
}