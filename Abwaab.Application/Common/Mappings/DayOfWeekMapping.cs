namespace Abwaab.Application.Common.Mappings;

public static class DayOfWeekMapping
{
    public static string Map(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Sunday => "الأحد",
            DayOfWeek.Monday => "الاثنين",
            DayOfWeek.Tuesday => "الثلاثاء",
            DayOfWeek.Wednesday => "الأربعاء",
            DayOfWeek.Thursday => "الخميس",
            DayOfWeek.Friday => "الجمعة",
            DayOfWeek.Saturday => "السبت",
            _ => ""
        };
    }
    public static string Map(int day)
    {
        return day switch
        {
            0 => "الأحد",
            1 => "الاثنين",
            2 => "الثلاثاء",
            3 => "الأربعاء",
            4 => "الخميس",
            5 => "الجمعة",
            6 => "السبت",
            _ => ""
        };
    }
}