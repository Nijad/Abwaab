using Abwaab.Application.Common.Enums;

namespace Abwaab.Application.Common.Mappings
{
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
    }
}
