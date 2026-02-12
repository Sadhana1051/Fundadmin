using FundAdministration.Api.DTOs;
using FundAdministration.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundAdministration.Api.Controllers;

/// <summary>CRUD operations for investment funds</summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class FundsController : ControllerBase
{
    private readonly IFundService _service;

    public FundsController(IFundService service) => _service = service;

    /// <summary>Get all funds</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FundReadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _service.GetAllAsync(ct));

    /// <summary>Get a fund by ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FundReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var fund = await _service.GetByIdAsync(id, ct);
        return fund is null ? NotFound() : Ok(fund);
    }

    /// <summary>Create a new fund</summary>
    [HttpPost]
    [ProducesResponseType(typeof(FundReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] FundCreateDto dto, CancellationToken ct)
    {
        var fund = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = fund.FundId }, fund);
    }

    /// <summary>Update an existing fund</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(FundReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] FundCreateDto dto, CancellationToken ct)
    {
        var fund = await _service.UpdateAsync(id, dto, ct);
        return fund is null ? NotFound() : Ok(fund);
    }

    /// <summary>Delete a fund</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }
}
