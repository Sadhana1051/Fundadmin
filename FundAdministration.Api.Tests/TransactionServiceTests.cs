using FundAdministration.Api.DTOs;
using FundAdministration.Api.Entities;
using FundAdministration.Api.Repositories;
using FundAdministration.Api.Services;
using Moq;
using Xunit;

namespace FundAdministration.Api.Tests;

public class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> _repoMock;
    private readonly TransactionService _sut;

    public TransactionServiceTests()
    {
        _repoMock = new Mock<ITransactionRepository>();
        _sut = new TransactionService(_repoMock.Object);
    }

    [Fact]
    public async Task RegisterTransactionAsync_Throws_When_Amount_Is_Negative()
    {
        var dto = new TransactionCreateDto
        {
            InvestorId = Guid.NewGuid(),
            Amount = -100,
            Type = TransactionType.Subscription
        };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.RegisterTransactionAsync(dto));

        Assert.Contains("positive", ex.Message);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RegisterTransactionAsync_Throws_When_Amount_Is_Zero()
    {
        var dto = new TransactionCreateDto
        {
            InvestorId = Guid.NewGuid(),
            Amount = 0,
            Type = TransactionType.Redemption
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.RegisterTransactionAsync(dto));
    }

    [Fact]
    public async Task RegisterTransactionAsync_Adds_And_Saves_When_Valid()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var dto = new TransactionCreateDto
        {
            InvestorId = Guid.NewGuid(),
            Amount = 1000,
            Type = TransactionType.Subscription
        };

        var result = await _sut.RegisterTransactionAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(dto.Amount, result.Amount);
        Assert.Equal(dto.Type, result.Type);
        _repoMock.Verify(r => r.AddAsync(It.Is<Transaction>(t => t.Amount == 1000 && t.Type == TransactionType.Subscription), It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
