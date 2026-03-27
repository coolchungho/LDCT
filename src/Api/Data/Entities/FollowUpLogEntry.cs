namespace LDCT.Api.Data.Entities;

public class FollowUpLogEntry
{
    public Guid Id { get; set; }
    public Guid CaseId { get; set; }
    public LdctCase Case { get; set; } = null!;

    /// <summary>追蹤軌道（胸腔門診／3／6／12 個月等）</summary>
    public string TrackCorridor { get; set; } = "";
    /// <summary>簡訊、電訪、其他</summary>
    public string ContactType { get; set; } = "";
    public DateOnly ContactDate { get; set; }
    public string? ContactResult { get; set; }
    public string Content { get; set; } = "";
    public Guid? LinkedSmsMessageId { get; set; }
    public string? HisQuerySummary { get; set; }

    public string CreatedByUserId { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
    public string? ModifiedByUserId { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? PreviousSnapshotJson { get; set; }
}
