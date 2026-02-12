using FundAdministration.Api.DTOs;
using FundAdministration.Api.Entities;
using FundAdministration.Api.Repositories;

namespace FundAdministration.Api.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;

    public TransactionService(ITransactionRepository repository) => _repository = repository;

    public async Task<TransactionReadDto> RegisterTransactionAsync(TransactionCreateDto dto, CancellationToken ct = default)
    {
        if (dto.Amount <= 0)
            throw new ArgumentException("Transaction amount must be positive", nameof(dto.Amount));

        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            InvestorId = dto.InvestorId,
            Type = dto.Type,
            Amount = dto.Amount,
            TransactionDate = DateTime.UtcNow
        };

        await _repository.AddAsync(transaction, ct);
        await _repository.SaveChangesAsync(ct);

        return new TransactionReadDto
        {
            TransactionId = transaction.TransactionId,
            Type = transaction.Type,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate
        };
    }

    public async Task<IEnumerable<TransactionReadDto>> GetTransactionsByInvestorAsync(Guid investorId, CancellationToken ct = default)
    {
        var transactions = await _repository.GetByInvestorAsync(investorId, ct);
        return transactions.Select(t => new TransactionReadDto
        {
            TransactionId = t.TransactionId,
            Type = t.Type,
            Amount = t.Amount,
            TransactionDate = t.TransactionDate
        });
    }

    public async Task<FundTotalsDto> GetTotalsByFundAsync(Guid fundId, CancellationToken ct = default)
    {
        var (subscribed, redeemed) = await _repository.GetTotalsByFundIdAsync(fundId, ct);
        return new FundTotalsDto(fundId, subscribed, redeemed);
    }
}