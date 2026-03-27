using LDCT.Api.Data.Entities;

namespace LDCT.Api.Data;

public static class DbSeed
{
    public static async Task SeedIntegrationDefaultsAsync(LdctDbContext db, CancellationToken ct = default)
    {
        var defaults = new Dictionary<string, string>
        {
            ["url.report_query"] = "https://ehr.example.invalid/report?mrn={mrn}&examDate={examDate}",
            ["url.basic_profile"] = "https://his.example.invalid/patient?mrn={mrn}",
            ["url.sms_portal"] = "/sms?caseId={caseId}",
            ["url.his_deep_link"] = "https://his.example.invalid/deep?mrn={mrn}",
            ["fetnet.endpoint"] = "https://umc.fetnet.net/api/mock",
            ["llm.enabled"] = "false"
        };

        foreach (var (k, v) in defaults)
        {
            if (await db.IntegrationSettings.FindAsync([k], ct) == null)
            {
                db.IntegrationSettings.Add(new IntegrationSettingRecord
                {
                    Key = k,
                    Value = v,
                    UpdatedAt = DateTimeOffset.UtcNow
                });
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
