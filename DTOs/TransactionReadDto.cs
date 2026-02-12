using FundAdministration.Api.Entities;

namespace FundAdministration.Api.DTOs;

public class TransactionReadDto
{
    public Guid TransactionId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
}