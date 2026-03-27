using LDCT.Api.Contracts;
using LDCT.Api.Data;
using LDCT.Api.Data.Entities;
using LDCT.Api.Security;
using LDCT.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LDCT.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class CasesController(LdctDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CaseListItemDto>>> List(
        [FromQuery] DateOnly? examFrom,
        [FromQuery] DateOnly? examTo,
        [FromQuery] bool? openOnly,
        [FromQuery] bool defaultDueThisMonth = true,
        CancellationToken cancellationToken = default)
    {
        var q = db.Cases.AsNoTracking().Where(c => c.ReportLanded);

        if (openOnly == true)
            q = q.Where(c => !c.IsClosed);

        if (examFrom.HasValue)
            q = q.Where(c => c.ExamDate >= examFrom.Value);
        if (examTo.HasValue)
            q = q.Where(c => c.ExamDate <= examTo.Value);

        var list = await q.OrderByDescending(c => c.ExamDate).ToListAsync(cancellationToken);

        if (defaultDueThisMonth)
            list = list.Where(CaseFilterService.IsDueThisMonth).ToList();

        return Ok(list.Select(Map).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CaseListItemDto>> Get(Guid id, CancellationToken ct)
    {
        var c = await db.Cases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return c == null ? NotFound() : Ok(Map(c));
    }

    [HttpGet("{id:guid}/report")]
    [Authorize(Roles = $"{AppRoles.CaseManager},{AppRoles.Physician},{AppRoles.Administrator}")]
    public async Task<ActionResult<ReportViewDto>> Report(Guid id, CancellationToken ct)
    {
        var c = await db.Cases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (c == null) return NotFound();
        var settings = await db.IntegrationSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Key == "url.report_query", ct);
        var hint = settings?.Value
            .Replace("{mrn}", Uri.EscapeDataString(c.MedicalRecordNumber))
            .Replace("{examDate}", Uri.EscapeDataString(c.ExamDate.ToString("yyyy-MM-dd")));
        return Ok(new ReportViewDto(
            c.ReportNarrativeSummary ?? c.CaseManagerNarrativeNotes,
            c.HasNodule,
            c.NoduleCount,
            c.MaxNoduleLengthMm,
            c.LlmNeedsReview,
            hint));
    }

    [HttpPost("{id:guid}/closure")]
    [Authorize(Roles = $"{AppRoles.CaseManager},{AppRoles.Administrator}")]
    public async Task<ActionResult> Close(Guid id, [FromBody] CloseCaseRequest body, CancellationToken ct)
    {
        if (body.ClosureCode is < 0 or > 6)
            return BadRequest("ClosureCode must be 0–6.");
        var c = await db.Cases.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (c == null) return NotFound();
        c.IsClosed = true;
        c.ClosureCode = body.ClosureCode;
        c.ClosedAt = DateTimeOffset.UtcNow;
        c.ClosedByUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        c.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    private static CaseListItemDto Map(LdctCase c) => new(
        c.Id,
        c.MedicalRecordNumber,
        c.PatientName,
        c.ExamDate,
        c.CustomerName,
        c.Phone,
        c.Mobile,
        c.TrackType,
        c.Track1,
        c.Track2,
        c.Track3,
        c.Track4,
        c.ChestClinicOneMonth,
        c.Track3Months,
        c.Track6Months,
        c.Track12Months,
        c.LdctStatus,
        c.IsClosed,
        c.ClosureCode,
        c.ReportLanded);
}
