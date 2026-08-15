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

        public static string Map(string state)
        {
            if(state==AppointmentStatesEnum.Pending.ToString())
                return Map(AppointmentStatesEnum.Pending);
            if(state==AppointmentStatesEnum.Refused.ToString())
                return Map(AppointmentStatesEnum.Refused);
            if(state==AppointmentStatesEnum.Accepted.ToString())
                return Map(AppointmentStatesEnum.Accepted);
            if(state==AppointmentStatesEnum.Canceled.ToString())
                return Map(AppointmentStatesEnum.Canceled);
            if(state==AppointmentStatesEnum.Unfinished.ToString())
                return Map(AppointmentStatesEnum.Unfinished);
            if(state==AppointmentStatesEnum.Completed.ToString())
                return Map(AppointmentStatesEnum.Completed);
            return "";
        }
    }
}
