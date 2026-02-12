using FundAdministration.Api.DTOs;
using FundAdministration.Api.Entities;
using FundAdministration.Api.Repositories;

namespace FundAdministration.Api.Services;

public class FundService : IFundService
{
    private readonly IFundRepository _repository;

    public FundService(IFundRepository repository) => _repository = repository;

    public async Task<FundReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var fund = await _repository.GetByIdAsync(id, ct);
        return fund is null ? null : ToDto(fund);
    }

    public async Task<IEnumerable<FundReadDto>> GetAllAsync(CancellationToken ct = default)
    {
        var funds = await _repository.GetAllAsync(ct);
        return funds.Select(ToDto);
    }

    public async Task<FundReadDto> CreateAsync(FundCreateDto dto, CancellationToken ct = default)
    {
        var fund = new Fund
        {
            FundId = Guid.NewGuid(),
            Name = dto.Name,
            Currency = dto.Currency,
            LaunchDate = dto.LaunchDate
        };
        await _repository.AddAsync(fund, ct);
        await _repository.SaveChangesAsync(ct);
        return ToDto(fund);
    }

    public async Task<FundReadDto?> UpdateAsync(Guid id, FundCreateDto dto, CancellationToken ct = default)
    {
        var fund = await _repository.GetByIdAsync(id, ct);
        if (fund is null) return null;
        fund.Name = dto.Name;
        fund.Currency = dto.Currency;
        fund.LaunchDate = dto.LaunchDate;
        _repository.Update(fund);
        await _repository.SaveChangesAsync(ct);
        return ToDto(fund);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var fund = await _repository.GetByIdAsync(id, ct);
        if (fund is null) return false;
        _repository.Remove(fund);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    private static FundReadDto ToDto(Fund f) =>
        new(f.FundId, f.Name, f.Currency, f.LaunchDate);
}
