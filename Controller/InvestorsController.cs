using FundAdministration.Api.DTOs;
using FundAdministration.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundAdministration.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class InvestorsController : ControllerBase
{
    private readonly IInvestorService _service;

    public InvestorsController(IInvestorService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InvestorReadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    [HttpGet("fund/{fundId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<InvestorReadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByFund(Guid fundId, CancellationToken ct) =>
        Ok(await _service.GetByFundIdAsync(fundId, ct));

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InvestorReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var investor = await _service.GetByIdAsync(id, ct);
        return investor is null ? NotFound() : Ok(investor);
    }

    [HttpPost]
    [ProducesResponseType(typeof(InvestorReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] InvestorCreateDto dto, CancellationToken ct)
    {
        var investor = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = investor.InvestorId }, investor);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(InvestorReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] InvestorUpdateDto dto, CancellationToken ct)
    {
        var investor = await _service.UpdateAsync(id, dto, ct);
        return investor is null ? NotFound() : Ok(investor);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }
}
