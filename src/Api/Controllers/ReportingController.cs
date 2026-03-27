using LDCT.Api.Data;
using LDCT.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LDCT.Api.Controllers;

[ApiController]
[Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.CaseManager}")]
[Route("api/v1/[controller]")]
public class ReportingController(LdctDbContext db) : ControllerBase
{
    /// <summary>報表資料集 JSON—語意與清單一致，供 Power BI／閘道後續銜接。</summary>
    [HttpGet("dataset")]
    public async Task<ActionResult<IReadOnlyList<object>>> Dataset(CancellationToken ct)
    {
        var rows = await db.Cases.AsNoTracking()
            .Where(c => c.ReportLanded)
            .OrderByDescending(c => c.ExamDate)
            .Select(c => new
            {
                c.Id,
                c.MedicalRecordNumber,
                c.PatientName,
                c.ExamDate,
                c.CustomerName,
                c.IsClosed,
                c.ClosureCode,
                c.ChestClinicOneMonth,
                c.Track3Months,
                c.Track6Months,
                c.Track12Months,
                c.LdctStatus,
                c.ReportLandedAt
            })
            .ToListAsync(ct);
        return Ok(rows);
    }
}
