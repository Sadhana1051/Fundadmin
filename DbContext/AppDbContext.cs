using FundAdministration.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundAdministration.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Fund> Funds => Set<Fund>();
    public DbSet<Investor> Investors => Set<Investor>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fund>(e =>
        {
            e.HasKey(x => x.FundId);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Currency).HasMaxLength(3).IsRequired();
        });

        modelBuilder.Entity<Investor>(e =>
        {
            e.HasKey(x => x.InvestorId);
            e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            e.Property(x => x.Email).HasMaxLength(256).IsRequired();
            e.HasOne(x => x.Fund).WithMany(f => f.Investors).HasForeignKey(x => x.FundId);
        });

        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasKey(x => x.TransactionId);
            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.HasOne(x => x.Investor).WithMany(i => i.Transactions).HasForeignKey(x => x.InvestorId);
        });
    }
}