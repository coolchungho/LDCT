using LDCT.Api.Data.Entities;

namespace LDCT.Api.Services;

public static class CaseFilterService
{
    public static DateOnly AddCalendarMonths(DateOnly exam, int months)
    {
        var d = exam.ToDateTime(TimeOnly.MinValue);
        var next = d.AddMonths(months);
        return DateOnly.FromDateTime(next);
    }

    /// <summary>
    /// 若任一已啟用之追蹤類型之應到日落於指定曆月，且未結案，則為 true（對齊 design / case-list）。
    /// </summary>
    public static bool IsDueInCalendarMonth(LdctCase c, int year, int month)
    {
        if (c.IsClosed) return false;
        var start = new DateOnly(year, month, 1);
        var end = start.AddMonths(1).AddDays(-1);

        bool InRange(DateOnly due) => due >= start && due <= end;

        if (c.ChestClinicOneMonth && InRange(AddCalendarMonths(c.ExamDate, 1))) return true;
        if (c.Track3Months && InRange(AddCalendarMonths(c.ExamDate, 3))) return true;
        if (c.Track6Months && InRange(AddCalendarMonths(c.ExamDate, 6))) return true;
        if (c.Track12Months && InRange(AddCalendarMonths(c.ExamDate, 12))) return true;
        return false;
    }

    public static bool IsDueThisMonth(LdctCase c) =>
        IsDueInCalendarMonth(c, DateTime.UtcNow.Year, DateTime.UtcNow.Month);
}
