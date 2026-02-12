using FundAdministration.Api.DTOs;

namespace FundAdministration.Api.Services;

public interface ITransactionService
{
    Task<TransactionReadDto> RegisterTransactionAsync(TransactionCreateDto dto, CancellationToken ct = default);
    Task<IEnumerable<TransactionReadDto>> GetTransactionsByInvestorAsync(Guid investorId, CancellationToken ct = default);
    Task<FundTotalsDto> GetTotalsByFundAsync(Guid fundId, CancellationToken ct = default);
}