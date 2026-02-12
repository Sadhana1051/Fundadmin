using FundAdministration.Api.DTOs;

namespace FundAdministration.Api.Services;

public interface IInvestorService
{
    Task<InvestorReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<InvestorReadDto>> GetByFundIdAsync(Guid fundId, CancellationToken ct = default);
    Task<IEnumerable<InvestorReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<InvestorReadDto> CreateAsync(InvestorCreateDto dto, CancellationToken ct = default);
    Task<InvestorReadDto?> UpdateAsync(Guid id, InvestorUpdateDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
