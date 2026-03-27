using LDCT.Api.Contracts;
using LDCT.Api.Data;
using LDCT.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LDCT.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class IntegrationsController(LdctDbContext db) : ControllerBase
{
    [HttpGet("urls")]
    [Authorize(Roles = $"{AppRoles.CaseManager},{AppRoles.Physician},{AppRoles.Administrator}")]
    public async Task<ActionResult<object>> ResolvedUrls([FromQuery] Guid caseId, CancellationToken ct)
    {
        var c = await db.Cases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == caseId, ct);
        if (c == null) return NotFound();

        async Task<string?> Expand(string key) =>
            (await db.IntegrationSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Key == key, ct))?.Value
                ?.Replace("{mrn}", Uri.EscapeDataString(c.MedicalRecordNumber))
                .Replace("{examDate}", Uri.EscapeDataString(c.ExamDate.ToString("yyyy-MM-dd")))
                .Replace("{caseId}", Uri.EscapeDataString(c.Id.ToString()));

        return Ok(new
        {
            reportQuery = await Expand("url.report_query"),
            basicProfile = await Expand("url.basic_profile"),
            smsPortal = await Expand("url.sms_portal")
        });
    }

    [HttpGet("his-link")]
    [Authorize(Roles = $"{AppRoles.CaseManager},{AppRoles.Administrator}")]
    public async Task<ActionResult<object>> HisLink([FromQuery] Guid caseId, CancellationToken ct)
    {
        var c = await db.Cases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == caseId, ct);
        if (c == null) return NotFound();
        var row = await db.IntegrationSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Key == "url.his_deep_link", ct);
        if (row == null) return NotFound("HIS URL template not configured.");
        var url = row.Value
            .Replace("{mrn}", Uri.EscapeDataString(c.MedicalRecordNumber))
            .Replace("{examDate}", Uri.EscapeDataString(c.ExamDate.ToString("yyyy-MM-dd")));
        return Ok(new { url });
    }

    /// <summary>LLM 擷取雛型；正式 PHI 須資安核定後啟用外部端點。</summary>
    [HttpPost("llm/extract")]
    [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Physician},{AppRoles.CaseManager}")]
    public ActionResult<LlmExtractResponse> LlmExtract([FromBody] LlmExtractRequest body)
    {
        var text = body.RawText ?? "";
        var needsReview = text.Length < 20;
        var hasNodule = text.Contains("結節", StringComparison.Ordinal) || text.Contains("nodule", StringComparison.OrdinalIgnoreCase);
        return Ok(new LlmExtractResponse(
            hasNodule,
            hasNodule ? 1 : 0,
            hasNodule ? 5.0m : null,
            text.Contains("門診", StringComparison.Ordinal),
            text.Contains("三個月", StringComparison.Ordinal) || text.Contains("3", StringComparison.Ordinal),
            text.Contains("六個月", StringComparison.Ordinal),
            text.Contains("一年", StringComparison.Ordinal) || text.Contains("12", StringComparison.Ordinal),
            needsReview,
            "mock-ldct-extractor/1.0"));
    }

    [HttpPost("survey/sync")]
    [Authorize(Roles = $"{AppRoles.Nurse},{AppRoles.Administrator}")]
    public ActionResult SurveySync([FromBody] object payload)
    {
        return Ok(new { accepted = true, note = "問卷同步雛型—實際介接依院方問卷系統。" });
    }
}
