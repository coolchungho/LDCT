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
[Authorize(Roles = $"{AppRoles.CaseManager},{AppRoles.Administrator}")]
[Route("api/v1/[controller]")]
public class SmsController(LdctDbContext db) : ControllerBase
{
    /// <summary>遠傳公務機流程之雛型：寫入紀錄並模擬送出；可連動追蹤紀錄。</summary>
    [HttpPost("send")]
    public async Task<ActionResult<SmsSendResponse>> Send([FromBody] SmsSendRequest body, [FromQuery] Guid caseId, CancellationToken ct)
    {
        var c = await db.Cases.FirstOrDefaultAsync(x => x.Id == caseId, ct);
        if (c == null) return NotFound();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown";
        var now = DateTimeOffset.UtcNow;

        var sms = new SmsMessage
        {
            Id = Guid.NewGuid(),
            CaseId = caseId,
            DestinationPhone = body.Phone,
            Body = body.Message,
            Status = "Sent",
            ProviderMessageId = Guid.NewGuid().ToString("N")[..12],
            SentAt = now,
            CreatedAt = now,
            CreatedByUserId = userId
        };
        db.SmsMessages.Add(sms);

        if (body.AutoCreateFollowUpLog)
        {
            db.FollowUpLogs.Add(new FollowUpLogEntry
            {
                Id = Guid.NewGuid(),
                CaseId = caseId,
                TrackCorridor = "SMS",
                ContactType = "簡訊",
                ContactDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ContactResult = "已發送",
                Content = body.Message,
                LinkedSmsMessageId = sms.Id,
                CreatedByUserId = userId,
                CreatedAt = now
            });
        }

        await db.SaveChangesAsync(ct);
        return Ok(new SmsSendResponse(sms.Id, sms.Status));
    }

    /// <summary>可用之簡訊內容範本（含變數佔位符）。</summary>
    [HttpGet("templates")]
    public ActionResult<IReadOnlyList<SmsTemplateDto>> Templates() =>
        Ok(SmsTemplateService.All.Select(t => new SmsTemplateDto(t.Key, t.Name, t.Body)).ToList());

    /// <summary>套用指定範本並以該個案資料替換變數，回傳可直接發送之內容。</summary>
    [HttpGet("templates/{key}/render")]
    public async Task<ActionResult<SmsTemplateDto>> RenderTemplate(string key, [FromQuery] Guid caseId, CancellationToken ct)
    {
        var template = SmsTemplateService.Find(key);
        if (template == null) return NotFound("Unknown template key.");
        var c = await db.Cases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == caseId, ct);
        if (c == null) return NotFound("Case not found.");
        return Ok(new SmsTemplateDto(template.Key, template.Name, SmsTemplateService.Render(template.Body, c)));
    }
}
