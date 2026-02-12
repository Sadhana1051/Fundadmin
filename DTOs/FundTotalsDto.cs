namespace FundAdministration.Api.DTOs;

public record FundTotalsDto(Guid FundId, decimal Subscribed, decimal Redeemed);
