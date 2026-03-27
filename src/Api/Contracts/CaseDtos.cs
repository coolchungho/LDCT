namespace LDCT.Api.Contracts;

public record CaseListQuery(
    DateOnly? ExamFrom,
    DateOnly? ExamTo,
    bool? DefaultDueThisMonth,
    bool? OpenOnly);

public record CaseListItemDto(
    Guid Id,
    string MedicalRecordNumber,
    string PatientName,
    DateOnly ExamDate,
    string? CustomerName,
    string? Phone,
    string? Mobile,
    string? TrackType,
    string? Track1,
    string? Track2,
    string? Track3,
    string? Track4,
    bool ChestClinicOneMonth,
    bool Track3Months,
    bool Track6Months,
    bool Track12Months,
    string? LdctStatus,
    bool IsClosed,
    int? ClosureCode,
    bool ReportLanded);

public record CloseCaseRequest(int ClosureCode);

public record RosterImportRow(
    string MedicalRecordNumber,
    string PatientName,
    string? Gender,
    int? Age,
    DateOnly ExamDate,
    string? CustomerName,
    string? Phone,
    string? Mobile);

public record EveningBatchReportRow(
    string MedicalRecordNumber,
    bool HasNodule,
    int? NoduleCount,
    decimal? MaxNoduleLengthMm,
    bool NeedOutpatientFollowUp,
    bool Track3Months,
    bool Track6Months,
    bool Track12Months,
    string? NarrativeSummary,
    bool IssuedReport);

public record LlmExtractRequest(string RawText);

public record LlmExtractResponse(
    bool HasNodule,
    int? NoduleCount,
    decimal? MaxNoduleLengthMm,
    bool NeedOutpatientFollowUp,
    bool Track3Months,
    bool Track6Months,
    bool Track12Months,
    bool NeedsHumanReview,
    string ModelVersion);

public record FollowUpCreateRequest(
    string TrackCorridor,
    string ContactType,
    DateOnly ContactDate,
    string? ContactResult,
    string Content,
    string? HisQuerySummary);

public record FollowUpPatchRequest(
    string? TrackCorridor,
    string? ContactType,
    DateOnly? ContactDate,
    string? ContactResult,
    string? Content,
    string? HisQuerySummary);

public record FollowUpItemDto(
    Guid Id,
    string TrackCorridor,
    string ContactType,
    DateOnly ContactDate,
    string? ContactResult,
    string Content,
    Guid? LinkedSmsMessageId,
    string? HisQuerySummary,
    string CreatedByUserId,
    DateTimeOffset CreatedAt,
    string? ModifiedByUserId,
    DateTimeOffset? ModifiedAt);

public record SmsSendRequest(string Phone, string Message, bool AutoCreateFollowUpLog);

public record SmsSendResponse(Guid SmsId, string Status);

public record ReportViewDto(
    string? Summary,
    bool? HasNodule,
    int? NoduleCount,
    decimal? MaxNoduleLengthMm,
    bool LlmNeedsReview,
    string? ExternalUrlHint);

public record ClinicalAlertDto(string Severity, string Code, string Message, Guid? CaseId, string? MedicalRecordNumber);
