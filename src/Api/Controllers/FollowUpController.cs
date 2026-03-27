using System.Text.Json;
using LDCT.Api.Contracts;
using LDCT.Api.Data;
using LDCT.Api.Data.Entities;
using LDCT.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LDCT.Api.Controllers;

[ApiController]
[Authorize(Roles = $"{AppRoles.CaseManager},{AppRoles.Nurse},{AppRoles.Physician},{AppRoles.Administrator}")]
[Route("api/v1/cases/{caseId:guid}/[controller]")]
public class FollowUpController(LdctDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<object>> GetCaseHeader(Guid caseId, CancellationToken ct)
    {
        var c = await db.Cases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == caseId, ct);
        if (c == null) return NotFound();
        return Ok(new
        {
            c.MedicalRecordNumber,
            c.PatientName,
            c.ExamDate
        });
    }

    [HttpGet("logs")]
    public async Task<ActionResult<IReadOnlyList<FollowUpItemDto>>> ListLogs(Guid caseId, CancellationToken ct)
    {
        if (!await db.Cases.AnyAsync(x => x.Id == caseId, ct)) return NotFound();
        var logs = await db.FollowUpLogs.AsNoTracking()
            .Where(x => x.CaseId == caseId)
            .OrderByDescending(x => x.ContactDate)
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new FollowUpItemDto(
                x.Id,
                x.TrackCorridor,
                x.ContactType,
                x.ContactDate,
                x.ContactResult,
                x.Content,
                x.LinkedSmsMessageId,
                x.HisQuerySummary,
                x.CreatedByUserId,
                x.CreatedAt,
                x.ModifiedByUserId,
                x.ModifiedAt))
            .ToListAsync(ct);
        return Ok(logs);
    }

    [HttpPost("logs")]
    public async Task<ActionResult<FollowUpItemDto>> CreateLog(Guid caseId, [FromBody] FollowUpCreateRequest body, CancellationToken ct)
    {
        var c = await db.Cases.FirstOrDefaultAsync(x => x.Id == caseId, ct);
        if (c == null) return NotFound();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown";
        var now = DateTimeOffset.UtcNow;
        var e = new FollowUpLogEntry
        {
            Id = Guid.NewGuid(),
            CaseId = caseId,
            TrackCorridor = body.TrackCorridor,
            ContactType = body.ContactType,
            ContactDate = body.ContactDate,
            ContactResult = body.ContactResult,
            Content = body.Content,
            HisQuerySummary = body.HisQuerySummary,
            CreatedByUserId = userId,
            CreatedAt = now
        };
        db.FollowUpLogs.Add(e);
        await db.SaveChangesAsync(ct);
        var dto = new FollowUpItemDto(
            e.Id, e.TrackCorridor, e.ContactType, e.ContactDate, e.ContactResult, e.Content,
            e.LinkedSmsMessageId, e.HisQuerySummary, e.CreatedByUserId, e.CreatedAt,
            e.ModifiedByUserId, e.ModifiedAt);
        return Created($"/api/v1/cases/{caseId}/FollowUp/logs/{e.Id}", dto);
    }

    [HttpPatch("logs/{logId:guid}")]
    public async Task<ActionResult<FollowUpItemDto>> PatchLog(Guid caseId, Guid logId, [FromBody] FollowUpPatchRequest body, CancellationToken ct)
    {
        var e = await db.FollowUpLogs.FirstOrDefaultAsync(x => x.CaseId == caseId && x.Id == logId, ct);
        if (e == null) return NotFound();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown";
        e.PreviousSnapshotJson = JsonSerializer.Serialize(new
        {
            e.TrackCorridor,
            e.ContactType,
            e.ContactDate,
            e.ContactResult,
            e.Content,
            e.HisQuerySummary
        });
        if (body.TrackCorridor != null) e.TrackCorridor = body.TrackCorridor;
        if (body.ContactType != null) e.ContactType = body.ContactType;
        if (body.ContactDate.HasValue) e.ContactDate = body.ContactDate.Value;
        if (body.ContactResult != null) e.ContactResult = body.ContactResult;
        if (body.Content != null) e.Content = body.Content;
        if (body.HisQuerySummary != null) e.HisQuerySummary = body.HisQuerySummary;
        e.ModifiedByUserId = userId;
        e.ModifiedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        return Ok(new FollowUpItemDto(
            e.Id, e.TrackCorridor, e.ContactType, e.ContactDate, e.ContactResult, e.Content,
            e.LinkedSmsMessageId, e.HisQuerySummary, e.CreatedByUserId, e.CreatedAt,
            e.ModifiedByUserId, e.ModifiedAt));
    }
}
