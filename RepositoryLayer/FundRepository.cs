using FundAdministration.Api.Data;
using FundAdministration.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundAdministration.Api.Repositories;

public class FundRepository : IFundRepository
{
    private readonly AppDbContext _context;

    public FundRepository(AppDbContext context) => _context = context;

    public async Task<Fund?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Funds.FindAsync([id], ct);

    public async Task<IEnumerable<Fund>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Funds.ToListAsync(ct);

    public async Task AddAsync(Fund fund, CancellationToken ct = default) =>
        await _context.Funds.AddAsync(fund, ct);

    public void Update(Fund fund) => _context.Funds.Update(fund);
    public void Remove(Fund fund) => _context.Funds.Remove(fund);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);
}
