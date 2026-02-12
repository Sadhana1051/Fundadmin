using FundAdministration.Api.DTOs;

namespace FundAdministration.Api.Services;

public interface IFundService
{
    Task<FundReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<FundReadDto>> GetAllAsync(CancellationToken ct = default);
    Task<FundReadDto> CreateAsync(FundCreateDto dto, CancellationToken ct = default);
    Task<FundReadDto?> UpdateAsync(Guid id, FundCreateDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
