using FundAdministration.Api.DTOs;
using FundAdministration.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundAdministration.Api.Controllers;

/// <summary>Transaction registration and queries</summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionsController(ITransactionService service) => _service = service;

    /// <summary>Register a new transaction (subscription or redemption)</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TransactionReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] TransactionCreateDto dto, CancellationToken ct)
    {
        var result = await _service.RegisterTransactionAsync(dto, ct);
        return CreatedAtAction(nameof(GetByInvestor), new { investorId = dto.InvestorId }, result);
    }

    /// <summary>Get all transactions for a specific investor</summary>
    [HttpGet("investor/{investorId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<TransactionReadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByInvestor(Guid investorId, CancellationToken ct) =>
        Ok(await _service.GetTransactionsByInvestorAsync(investorId, ct));

    /// <summary>Get total subscribed and redeemed amounts for a fund</summary>
    [HttpGet("fund/{fundId:guid}/totals")]
    [ProducesResponseType(typeof(FundTotalsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFundTotals(Guid fundId, CancellationToken ct) =>
        Ok(await _service.GetTotalsByFundAsync(fundId, ct));
}