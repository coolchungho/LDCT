namespace LDCT.Api.Data.Entities;

/// <summary>晚間批次：報告病歷號無對應個案時記錄例外</summary>
public class OrphanLdctReportRow
{
    public Guid Id { get; set; }
    public string MedicalRecordNumber { get; set; } = "";
    public DateTimeOffset ReceivedAt { get; set; }
    public string PayloadSummary { get; set; } = "";
    public string Reason { get; set; } = "NoMatchingCase";
}
