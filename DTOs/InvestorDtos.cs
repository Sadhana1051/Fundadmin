namespace FundAdministration.Api.DTOs;

/// <summary>Investor create payload</summary>
public record InvestorCreateDto(string FullName, string Email, Guid FundId);

/// <summary>Investor update payload</summary>
public record InvestorUpdateDto(string FullName, string Email);

/// <summary>Investor response</summary>
public record InvestorReadDto(Guid InvestorId, string FullName, string Email, Guid FundId);
