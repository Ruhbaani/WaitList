using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WaitListWeb.Models;

namespace WaitListWeb.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Keep your existing entities in the same DbContext
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Queue> Queues => Set<Queue>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Account> Accounts => Set<Account>();
}