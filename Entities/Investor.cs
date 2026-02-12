namespace FundAdministration.Api.Entities;

public class Investor
{
    public Guid InvestorId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid FundId { get; set; }

    public Fund Fund { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}