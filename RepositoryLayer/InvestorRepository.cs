using FundAdministration.Api.Data;
using FundAdministration.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundAdministration.Api.Repositories;

public class InvestorRepository : IInvestorRepository
{
    private readonly AppDbContext _context;

    public InvestorRepository(AppDbContext context) => _context = context;

    public async Task<Investor?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Investors.Include(i => i.Fund).FirstOrDefaultAsync(i => i.InvestorId == id, ct);

    public async Task<IEnumerable<Investor>> GetByFundIdAsync(Guid fundId, CancellationToken ct = default) =>
        await _context.Investors.Where(i => i.FundId == fundId).ToListAsync(ct);

    public async Task<IEnumerable<Investor>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Investors.Include(i => i.Fund).ToListAsync(ct);

    public async Task AddAsync(Investor investor, CancellationToken ct = default) =>
        await _context.Investors.AddAsync(investor, ct);

    public void Update(Investor investor) => _context.Investors.Update(investor);
    public void Remove(Investor investor) => _context.Investors.Remove(investor);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);
}
