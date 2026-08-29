using Abwaab.Application.Common.Enums;
namespace Abwaab.Application.Common.Mappings;

public static class UserPlanStatesMapping
{
    public static string Map(UserPlanStatesEnum state)
    {
        return state switch
        {
            UserPlanStatesEnum.Pending => "بالانتظار",
            UserPlanStatesEnum.Active => "فعال",
            UserPlanStatesEnum.Working => "يعمل",
            UserPlanStatesEnum.Expiered => "منتهي الصلاحية",
            UserPlanStatesEnum.Canceled => "ملغى",
            _ => ""
        };
    }

    public static string Map(string state)
    {
        if (state == UserPlanStatesEnum.Pending.ToString())
            return Map(UserPlanStatesEnum.Pending);
        if (state == UserPlanStatesEnum.Active.ToString())
            return Map(UserPlanStatesEnum.Active);
        if (state == UserPlanStatesEnum.Working.ToString())
            return Map(UserPlanStatesEnum.Working);
        if (state == UserPlanStatesEnum.Expiered.ToString())
            return Map(UserPlanStatesEnum.Expiered);
        if (state == UserPlanStatesEnum.Canceled.ToString())
            return Map(UserPlanStatesEnum.Canceled);
        return "";
    }
}
