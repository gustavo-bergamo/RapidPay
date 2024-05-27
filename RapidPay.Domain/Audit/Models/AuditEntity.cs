namespace RapidPay.Domain.Audit.Models;
public abstract class AuditEntity
{
    public int CreatedUser { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public string CreatedIPAddress { get; set; }

    public int ModifiedUser { get; set; }
    public DateTime ModifiedOnUtc { get; set; }
    public string ModifiedIPAddress { get; set; }
}