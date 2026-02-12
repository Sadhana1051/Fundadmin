namespace FundAdministration.Api.DTOs;

/// <summary>Fund create/update payload</summary>
public record FundCreateDto(string Name, string Currency, DateTime LaunchDate);

/// <summary>Fund response</summary>
public record FundReadDto(Guid FundId, string Name, string Currency, DateTime LaunchDate);

/// <summary>Fund report: net investment and investor count</summary>
public record FundReportDto(Guid FundId, string FundName, int InvestorCount, decimal NetInvestment);
