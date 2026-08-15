using Abwaab.Application.Common.Enums;

namespace Abwaab.Application.Common.Mappings
{
    public static class AppointmentStatesMapping
    {
        public static string Map(AppointmentStatesEnum state)
        {
            return state switch
            {
                AppointmentStatesEnum.Pending => "بالانتظار",
                AppointmentStatesEnum.Refused => "مرفوض",
                AppointmentStatesEnum.Accepted => "مثبت",
                AppointmentStatesEnum.Canceled => "ملغى",
                AppointmentStatesEnum.Unfinished => "لم يكتمل",
                AppointmentStatesEnum.Completed => "مكتمل",
                _ => ""
            };
        }
    }
}
