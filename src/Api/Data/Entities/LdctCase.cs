namespace LDCT.Api.Data.Entities;

public class LdctCase
{
    public Guid Id { get; set; }

    /// <summary>病歷號（個案主鍵）</summary>
    public string MedicalRecordNumber { get; set; } = "";

    public string PatientName { get; set; } = "";
    public string? Gender { get; set; }
    public int? Age { get; set; }
    public DateOnly ExamDate { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }

    public string? TrackType { get; set; }
    public string? Track1 { get; set; }
    public string? Track2 { get; set; }
    public string? Track3 { get; set; }
    public string? Track4 { get; set; }

    /// <summary>胸腔門診（1 個月）追蹤旗標</summary>
    public bool ChestClinicOneMonth { get; set; }
    public bool Track3Months { get; set; }
    public bool Track6Months { get; set; }
    public bool Track12Months { get; set; }

    public string? LdctStatus { get; set; }
    public bool IsClosed { get; set; }
    /// <summary>結案代碼 0–6，見 case-closure spec</summary>
    public int? ClosureCode { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }
    public string? ClosedByUserId { get; set; }

    // LDCT 報告（結構化）
    public bool? HasNodule { get; set; }
    public int? NoduleCount { get; set; }
    public decimal? MaxNoduleLengthMm { get; set; }
    public bool? NeedOutpatientFollowUp { get; set; }
    public string? ReportNarrativeSummary { get; set; }
    public string? ExternalReportQueryUrlTemplateKey { get; set; }
    public bool ReportLanded { get; set; }
    public DateTimeOffset? ReportLandedAt { get; set; }

    public string? LlmRawOutputJson { get; set; }
    public string? LlmVerifiedByUserId { get; set; }
    public bool LlmNeedsReview { get; set; }

    public string? CaseManagerNarrativeNotes { get; set; }

    // 確診登錄（簡化）
    public bool DiagnosisRegistered { get; set; }
    public string? DiagnosisStage { get; set; }
    public DateOnly? DiagnosisDate { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? CreatedByUserId { get; set; }
    public string? UpdatedByUserId { get; set; }

    public ICollection<FollowUpLogEntry> FollowUpLogs { get; set; } = new List<FollowUpLogEntry>();
    public ICollection<SmsMessage> SmsMessages { get; set; } = new List<SmsMessage>();
}
