using FundAdministration.Api.Data;
using FundAdministration.Api.Entities;
using FundAdministration.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FundAdministration.Api.Tests;

public class TransactionRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly TransactionRepository _sut;

    public TransactionRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _sut = new TransactionRepository(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task GetTotalsByFundIdAsync_Returns_Subscribed_And_Redeemed()
    {
        var fund = new Fund { FundId = Guid.NewGuid(), Name = "Test", Currency = "USD", LaunchDate = DateTime.UtcNow };
        var investor = new Investor { InvestorId = Guid.NewGuid(), FullName = "Test", Email = "t@t.com", FundId = fund.FundId };
        _context.Funds.Add(fund);
        _context.Investors.Add(investor);
        _context.Transactions.AddRange(
            new Transaction { TransactionId = Guid.NewGuid(), InvestorId = investor.InvestorId, Type = TransactionType.Subscription, Amount = 100, TransactionDate = DateTime.UtcNow },
            new Transaction { TransactionId = Guid.NewGuid(), InvestorId = investor.InvestorId, Type = TransactionType.Subscription, Amount = 200, TransactionDate = DateTime.UtcNow },
            new Transaction { TransactionId = Guid.NewGuid(), InvestorId = investor.InvestorId, Type = TransactionType.Redemption, Amount = 50, TransactionDate = DateTime.UtcNow }
        );
        await _context.SaveChangesAsync();

        var (subscribed, redeemed) = await _sut.GetTotalsByFundIdAsync(fund.FundId);

        Assert.Equal(300, subscribed);
        Assert.Equal(50, redeemed);
    }

    [Fact]
    public async Task GetByInvestorAsync_Returns_Only_That_Investors_Transactions()
    {
        var fund = new Fund { FundId = Guid.NewGuid(), Name = "F", Currency = "USD", LaunchDate = DateTime.UtcNow };
        var inv1 = new Investor { InvestorId = Guid.NewGuid(), FullName = "I1", Email = "i1@x.com", FundId = fund.FundId };
        var inv2 = new Investor { InvestorId = Guid.NewGuid(), FullName = "I2", Email = "i2@x.com", FundId = fund.FundId };
        _context.Funds.Add(fund);
        _context.Investors.AddRange(inv1, inv2);
        _context.Transactions.AddRange(
            new Transaction { TransactionId = Guid.NewGuid(), InvestorId = inv1.InvestorId, Type = TransactionType.Subscription, Amount = 100, TransactionDate = DateTime.UtcNow },
            new Transaction { TransactionId = Guid.NewGuid(), InvestorId = inv2.InvestorId, Type = TransactionType.Subscription, Amount = 200, TransactionDate = DateTime.UtcNow }
        );
        await _context.SaveChangesAsync();

        var result = await _sut.GetByInvestorAsync(inv1.InvestorId);

        Assert.Single(result);
        Assert.Equal(100, result.First().Amount);
    }
}
