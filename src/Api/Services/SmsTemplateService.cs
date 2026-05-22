using LDCT.Api.Data.Entities;

namespace LDCT.Api.Services;

/// <summary>
/// 簡訊內容範本目錄與變數替換（對齊 sms-fetnet「簡訊內容範例」）。
/// 變數：{name} 姓名、{examDate} 檢查日期、{interval} 建議追蹤期間。
/// </summary>
public static class SmsTemplateService
{
    public record Template(string Key, string Name, string Body);

    private static readonly Template[] Catalog =
    [
        new("ldct-followup",
            "LDCT 追蹤提醒",
            "親愛的貴賓您好：您（{name}）於 {examDate} 在新光醫院接受低劑量肺部電腦斷層檢查，請依醫師建議於 {interval} 追蹤。"),
        new("ldct-generic",
            "一般回診提醒",
            "【LDCT 追蹤提醒】{name} 您好，請依約回診或聯繫個管師。"),
    ];

    public static IReadOnlyList<Template> All => Catalog;

    public static Template? Find(string key) => Catalog.FirstOrDefault(t => t.Key == key);

    public static string Render(string body, LdctCase c) => body
        .Replace("{name}", c.PatientName)
        .Replace("{examDate}", c.ExamDate.ToString("yyyy-MM-dd"))
        .Replace("{interval}", IntervalText(c));

    private static string IntervalText(LdctCase c)
    {
        var parts = new List<string>();
        if (c.ChestClinicOneMonth) parts.Add("1 個月（胸腔門診）");
        if (c.Track3Months) parts.Add("3 個月");
        if (c.Track6Months) parts.Add("6 個月");
        if (c.Track12Months) parts.Add("12 個月");
        return parts.Count > 0 ? string.Join("、", parts) : "3–6 個月";
    }
}
