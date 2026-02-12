using FundAdministration.Api.Entities;

namespace FundAdministration.Api.DTOs;

public class TransactionCreateDto
{
    public Guid InvestorId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
}