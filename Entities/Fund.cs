namespace FundAdministration.Api.Entities;

public class Fund
{
    public Guid FundId { get; set; }
    public string Name { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public DateTime LaunchDate { get; set; }

    public ICollection<Investor> Investors { get; set; } = new List<Investor>();
}