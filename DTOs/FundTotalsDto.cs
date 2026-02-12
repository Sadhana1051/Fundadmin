namespace FundAdministration.Api.DTOs;

/// <summary>Total subscribed and redeemed amounts for a fund</summary>
public record FundTotalsDto(Guid FundId, decimal Subscribed, decimal Redeemed);
