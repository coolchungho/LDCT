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
[Authorize(Roles = $"{AppRoles.CaseManager},{AppRoles.Administrator}")]
[Route("api/v1/[controller]")]
public class ImportsController(LdctDbContext db) : ControllerBase
{
    /// <summary>健檢名單匯入（JSON 陣列；重複病歷號於同一批次阻擋）</summary>
    [HttpPost("roster")]
    public async Task<ActionResult> ImportRoster([FromBody] List<RosterImportRow> rows, CancellationToken ct)
    {
        if (rows.Count == 0) return BadRequest("Empty import.");
        var dup = rows.GroupBy(r => r.MedicalRecordNumber.Trim()).FirstOrDefault(g => g.Count() > 1);
        if (dup != null)
            return BadRequest($"Duplicate MRN in file: {dup.Key}");

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "system";
        var now = DateTimeOffset.UtcNow;

        foreach (var row in rows)
        {
            var mrn = row.MedicalRecordNumber.Trim();
            if (string.IsNullOrWhiteSpace(mrn) || string.IsNullOrWhiteSpace(row.PatientName))
                return BadRequest($"Invalid row for MRN {mrn}.");

            var existing = await db.Cases.FirstOrDefaultAsync(c => c.MedicalRecordNumber == mrn, ct);
            if (existing != null)
            {
                existing.PatientName = row.PatientName;
                existing.Gender = row.Gender;
                existing.Age = row.Age;
                existing.ExamDate = row.ExamDate;
                existing.CustomerName = row.CustomerName;
                existing.Phone = row.Phone;
                existing.Mobile = row.Mobile;
                existing.UpdatedAt = now;
                existing.UpdatedByUserId = userId;
            }
            else
            {
                db.Cases.Add(new LdctCase
                {
                    Id = Guid.NewGuid(),
                    MedicalRecordNumber = mrn,
                    PatientName = row.PatientName,
                    Gender = row.Gender,
                    Age = row.Age,
                    ExamDate = row.ExamDate,
                    CustomerName = row.CustomerName,
                    Phone = row.Phone,
                    Mobile = row.Mobile,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedByUserId = userId,
                    UpdatedByUserId = userId
                });
            }
        }

        await db.SaveChangesAsync(ct);
        return Ok(new { imported = rows.Count });
    }

    /// <summary>模擬「已發報告當日晚間批次」落地（EHR → 營運庫）</summary>
    [HttpPost("evening-batch")]
    public async Task<ActionResult> EveningBatch([FromBody] List<EveningBatchReportRow> reports, CancellationToken ct)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "system";
        var landedAt = DateTimeOffset.UtcNow;
        var orphans = 0;
        var updated = 0;

        foreach (var r in reports)
        {
            if (!r.IssuedReport)
                continue;

            var mrn = r.MedicalRecordNumber.Trim();
            var c = await db.Cases.FirstOrDefaultAsync(x => x.MedicalRecordNumber == mrn, ct);
            if (c == null)
            {
                db.OrphanLdctReports.Add(new OrphanLdctReportRow
                {
                    Id = Guid.NewGuid(),
                    MedicalRecordNumber = mrn,
                    ReceivedAt = landedAt,
                    PayloadSummary = JsonSerializer.Serialize(r),
                    Reason = "NoMatchingCase"
                });
                orphans++;
                continue;
            }

            c.HasNodule = r.HasNodule;
            c.NoduleCount = r.NoduleCount;
            c.MaxNoduleLengthMm = r.MaxNoduleLengthMm;
            c.NeedOutpatientFollowUp = r.NeedOutpatientFollowUp;
            c.ChestClinicOneMonth = r.NeedOutpatientFollowUp;
            c.Track3Months = r.Track3Months;
            c.Track6Months = r.Track6Months;
            c.Track12Months = r.Track12Months;
            c.ReportNarrativeSummary = r.NarrativeSummary;
            c.LdctStatus = "ReportIssued";
            c.ReportLanded = true;
            c.ReportLandedAt = landedAt;
            c.UpdatedAt = landedAt;
            c.UpdatedByUserId = userId;
            updated++;
        }

        await db.SaveChangesAsync(ct);
        return Ok(new { landedAt, updated, orphans });
    }

    [HttpGet("orphans")]
    public async Task<ActionResult> Orphans(CancellationToken ct)
    {
        var rows = await db.OrphanLdctReports.AsNoTracking()
            .OrderByDescending(x => x.ReceivedAt)
            .Take(500)
            .ToListAsync(ct);
        return Ok(rows);
    }
}
