using LDCT.Api.Contracts;
using LDCT.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace LDCT.Api.Services;

public static class ClinicalAlertEvaluator
{
    private static readonly string[] PathologyKeywords = ["恶性", "惡性", "腺癌", "鳞癌", "鱗癌", "carcinoma", "malignant"];

    public static async Task<List<ClinicalAlertDto>> EvaluateAsync(LdctDbContext db, CancellationToken ct)
    {
        var list = new List<ClinicalAlertDto>();

        var openCases = await db.Cases.AsNoTracking()
            .Where(c => c.ReportLanded && !c.IsClosed)
            .ToListAsync(ct);

        foreach (var c in openCases)
        {
            var due = CaseFilterService.IsDueThisMonth(c);
            var examPlus12 = CaseFilterService.AddCalendarMonths(c.ExamDate, 12);
            if (c.Track12Months && DateOnly.FromDateTime(DateTime.UtcNow) > examPlus12.AddMonths(1))
            {
                list.Add(new ClinicalAlertDto("warning", "followup_overdue", "12 個月追蹤可能逾期", c.Id, c.MedicalRecordNumber));
            }
            else if (due && c.Track3Months)
            {
                // 簡化：若本月應追蹤且 3m 旗標，提示積極追蹤（示範規則）
                list.Add(new ClinicalAlertDto("info", "due_this_month", "本月應追蹤（含 3 個月類型）", c.Id, c.MedicalRecordNumber));
            }
        }

        var recentLogs = await db.FollowUpLogs.AsNoTracking()
            .Where(l => l.Content != null)
            .OrderByDescending(l => l.CreatedAt)
            .Take(200)
            .Select(l => new { l.CaseId, l.Content })
            .ToListAsync(ct);

        foreach (var log in recentLogs)
        {
            if (log.Content != null && PathologyKeywords.Any(k => log.Content.Contains(k, StringComparison.OrdinalIgnoreCase)))
                list.Add(new ClinicalAlertDto("critical", "pathology_keyword", "追蹤內容含病理相關關鍵字，請複核。", log.CaseId, null));
        }

        return list;
    }
}
