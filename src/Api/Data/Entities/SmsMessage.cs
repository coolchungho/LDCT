namespace LDCT.Api.Data.Entities;

public class SmsMessage
{
    public Guid Id { get; set; }
    public Guid CaseId { get; set; }
    public LdctCase Case { get; set; } = null!;

    public string DestinationPhone { get; set; } = "";
    public string Body { get; set; } = "";
    public string Status { get; set; } = "Queued"; // Queued, Sent, Failed
    public string? ProviderMessageId { get; set; }
    public string? ErrorDetail { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }
    public string CreatedByUserId { get; set; } = "";
}
