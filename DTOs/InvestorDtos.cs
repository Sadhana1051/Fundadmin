namespace FundAdministration.Api.DTOs;

public record InvestorCreateDto(string FullName, string Email, Guid FundId);

public record InvestorUpdateDto(string FullName, string Email);

public record InvestorReadDto(Guid InvestorId, string FullName, string Email, Guid FundId);
