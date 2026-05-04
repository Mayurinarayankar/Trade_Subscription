using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace TradeSubscription.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<SubscriptionPlanConfig> SubscriptionPlans => Set<SubscriptionPlanConfig>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Incoterm> Incoterms => Set<Incoterm>();
    public DbSet<Trade> Trades => Set<Trade>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //  Company
        modelBuilder.Entity<Company>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Email).IsRequired().HasMaxLength(150);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Phone).HasMaxLength(20);
            e.Property(x => x.TaxNumber).HasMaxLength(50);
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        //  User
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).IsRequired().HasMaxLength(150);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            e.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            e.Property(x => x.PasswordHash).IsRequired();
            e.HasQueryFilter(x => !x.IsDeleted);

            e.HasOne(x => x.Company)
             .WithMany(c => c.Users)
             .HasForeignKey(x => x.CompanyId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        //   Subscription Plan 
        modelBuilder.Entity<SubscriptionPlanConfig>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.MonthlyPrice).HasPrecision(18, 2);
            e.Property(x => x.YearlyPrice).HasPrecision(18, 2);
        });

        //  Subscription 
        modelBuilder.Entity<Subscription>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.AmountPaid).HasPrecision(18, 2);
            e.HasQueryFilter(x => !x.IsDeleted);

            e.HasOne(x => x.Company)
             .WithMany(c => c.Subscriptions)
             .HasForeignKey(x => x.CompanyId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.PlanConfig)
             .WithMany(p => p.Subscriptions)
             .HasForeignKey(x => x.PlanConfigId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        //  Incoterm
        modelBuilder.Entity<Incoterm>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Code).IsRequired().HasMaxLength(10);
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
        });

        //  Trade
        modelBuilder.Entity<Trade>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.TradeNumber).IsRequired().HasMaxLength(50);
            e.HasIndex(x => x.TradeNumber).IsUnique();
            e.Property(x => x.UnitPrice).HasPrecision(18, 4);
            e.Property(x => x.TotalValue).HasPrecision(18, 4);
            e.Property(x => x.Quantity).HasPrecision(18, 4);
            e.Property(x => x.Currency).HasMaxLength(10);
            e.HasQueryFilter(x => !x.IsDeleted);

            e.HasOne(x => x.Company)
             .WithMany(c => c.Trades)
             .HasForeignKey(x => x.CompanyId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Incoterm)
             .WithMany(i => i.Trades)
             .HasForeignKey(x => x.IncotermId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        //  Invoice
        modelBuilder.Entity<Invoice>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.InvoiceNumber).IsRequired().HasMaxLength(50);
            e.HasIndex(x => x.InvoiceNumber).IsUnique();
            e.Property(x => x.SubTotal).HasPrecision(18, 2);
            e.Property(x => x.TaxAmount).HasPrecision(18, 2);
            e.Property(x => x.Discount).HasPrecision(18, 2);
            e.Property(x => x.TotalAmount).HasPrecision(18, 2);
            e.HasQueryFilter(x => !x.IsDeleted);

            e.HasOne(x => x.Company)
             .WithMany(c => c.Invoices)
             .HasForeignKey(x => x.CompanyId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Trade)
             .WithMany(t => t.Invoices)
             .HasForeignKey(x => x.TradeId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Subscription)
             .WithMany(s => s.Invoices)
             .HasForeignKey(x => x.SubscriptionId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Seed Data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder mb)
    {
        mb.Entity<SubscriptionPlanConfig>().HasData(
            new SubscriptionPlanConfig { Id = 1, Name = "Basic", Plan = WebApplication1.enums.SubscriptionPlan.Basic, MonthlyPrice = 29, YearlyPrice = 290, MaxUsers = 5, MaxTrades = 50, Features = "[\"5 Users\",\"50 Trades/month\",\"Basic Reports\",\"Email Support\"]", IsActive = true },
            new SubscriptionPlanConfig { Id = 2, Name = "Standard", Plan = WebApplication1.enums.SubscriptionPlan.Standard, MonthlyPrice = 79, YearlyPrice = 790, MaxUsers = 20, MaxTrades = 200, Features = "[\"20 Users\",\"200 Trades/month\",\"Advanced Reports\",\"Priority Support\",\"API Access\"]", IsActive = true },
            new SubscriptionPlanConfig { Id = 3, Name = "Premium", Plan = WebApplication1.enums.SubscriptionPlan.Premium, MonthlyPrice = 199, YearlyPrice = 1990, MaxUsers = 100, MaxTrades = 1000, Features = "[\"100 Users\",\"1000 Trades/month\",\"Full Analytics\",\"24/7 Support\",\"API Access\",\"Custom Integrations\"]", IsActive = true },
            new SubscriptionPlanConfig { Id = 4, Name = "Enterprise", Plan = WebApplication1.enums.SubscriptionPlan.Enterprise, MonthlyPrice = 499, YearlyPrice = 4990, MaxUsers = 9999, MaxTrades = 9999, Features = "[\"Unlimited Users\",\"Unlimited Trades\",\"Custom Reports\",\"Dedicated Support\",\"Full API\",\"SLA\",\"On-premise option\"]", IsActive = true }
        );

        mb.Entity<Incoterm>().HasData(
            new Incoterm { Id = 1, Code = "EXW", Name = "Ex Works", Description = "Seller makes goods available at their premises.", Type = WebApplication1.enums.IncotermsType.EXW, IsActive = true },
            new Incoterm { Id = 2, Code = "FOB", Name = "Free on Board", Description = "Seller delivers goods on board the vessel.", Type = WebApplication1.enums.IncotermsType.FOB, IsActive = true },
            new Incoterm { Id = 3, Code = "CIF", Name = "Cost Insurance Freight", Description = "Seller pays cost, insurance, and freight.", Type = WebApplication1.enums.IncotermsType.CIF, IsActive = true },
            new Incoterm { Id = 4, Code = "DDP", Name = "Delivered Duty Paid", Description = "Seller delivers goods cleared for import.", Type = WebApplication1.enums.IncotermsType.DDP, IsActive = true },
            new Incoterm { Id = 5, Code = "FCA", Name = "Free Carrier", Description = "Seller delivers to named carrier.", Type = WebApplication1.enums.IncotermsType.FCA, IsActive = true }
        );
    }
}