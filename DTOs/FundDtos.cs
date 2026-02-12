namespace FundAdministration.Api.DTOs;

public record FundCreateDto(string Name, string Currency, DateTime LaunchDate);

public record FundReadDto(Guid FundId, string Name, string Currency, DateTime LaunchDate);

public record FundReportDto(Guid FundId, string FundName, int InvestorCount, decimal NetInvestment);
