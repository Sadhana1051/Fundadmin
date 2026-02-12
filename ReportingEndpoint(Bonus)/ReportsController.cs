using FundAdministration.Api.Data;
using FundAdministration.Api.DTOs;
using FundAdministration.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FundAdministration.Api.Controllers;

/// <summary>Reporting endpoints: net investment and investor counts per fund</summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context) => _context = context;

    /// <summary>Get report: net investment and investor count per fund</summary>
    [HttpGet("funds")]
    [ProducesResponseType(typeof(IEnumerable<FundReportDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFundReport(CancellationToken ct)
    {
        var report = await _context.Funds
            .Select(f => new FundReportDto(
                f.FundId,
                f.Name,
                f.Investors.Count,
                f.Investors.SelectMany(i => i.Transactions)
                    .Sum(t => t.Type == TransactionType.Subscription ? t.Amount : -t.Amount)))
            .ToListAsync(ct);
        return Ok(report);
    }
}