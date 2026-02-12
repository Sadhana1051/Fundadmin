using FundAdministration.Api.DTOs;
using FundAdministration.Api.Entities;
using FundAdministration.Api.Repositories;

namespace FundAdministration.Api.Services;

public class InvestorService : IInvestorService
{
    private readonly IInvestorRepository _investorRepo;
    private readonly IFundRepository _fundRepo;

    public InvestorService(IInvestorRepository investorRepo, IFundRepository fundRepo)
    {
        _investorRepo = investorRepo;
        _fundRepo = fundRepo;
    }

    public async Task<InvestorReadDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var inv = await _investorRepo.GetByIdAsync(id, ct);
        return inv is null ? null : ToDto(inv);
    }

    public async Task<IEnumerable<InvestorReadDto>> GetByFundIdAsync(Guid fundId, CancellationToken ct = default)
    {
        var investors = await _investorRepo.GetByFundIdAsync(fundId, ct);
        return investors.Select(ToDto);
    }

    public async Task<IEnumerable<InvestorReadDto>> GetAllAsync(CancellationToken ct = default)
    {
        var investors = await _investorRepo.GetAllAsync(ct);
        return investors.Select(ToDto);
    }

    public async Task<InvestorReadDto> CreateAsync(InvestorCreateDto dto, CancellationToken ct = default)
    {
        var fund = await _fundRepo.GetByIdAsync(dto.FundId, ct);
        if (fund is null)
            throw new InvalidOperationException($"Fund with id {dto.FundId} not found.");

        var investor = new Investor
        {
            InvestorId = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            FundId = dto.FundId
        };
        await _investorRepo.AddAsync(investor, ct);
        await _investorRepo.SaveChangesAsync(ct);
        return ToDto(investor);
    }

    public async Task<InvestorReadDto?> UpdateAsync(Guid id, InvestorUpdateDto dto, CancellationToken ct = default)
    {
        var investor = await _investorRepo.GetByIdAsync(id, ct);
        if (investor is null) return null;
        investor.FullName = dto.FullName;
        investor.Email = dto.Email;
        _investorRepo.Update(investor);
        await _investorRepo.SaveChangesAsync(ct);
        return ToDto(investor);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var investor = await _investorRepo.GetByIdAsync(id, ct);
        if (investor is null) return false;
        _investorRepo.Remove(investor);
        await _investorRepo.SaveChangesAsync(ct);
        return true;
    }

    private static InvestorReadDto ToDto(Investor i) =>
        new(i.InvestorId, i.FullName, i.Email, i.FundId);
}
