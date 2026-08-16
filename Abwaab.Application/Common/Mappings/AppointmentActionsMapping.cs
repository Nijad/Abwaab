using Abwaab.Application.Common.Enums;

namespace Abwaab.Application.Common.Mappings
{
    public static class AppointmentActionsMapping
    {
        public static string Map(AppointmentActionsEnum action)
        {
            return action switch
            {
                AppointmentActionsEnum.Accept => "وافق",
                AppointmentActionsEnum.Cancel => "ألغى",
                AppointmentActionsEnum.Refuse => "رفض",
                AppointmentActionsEnum.Request => "طلب",
                AppointmentActionsEnum.Report => "بلّغ",
                AppointmentActionsEnum.Visit => "زار",
                _ => ""
            };
        }

        public static string Map(string action)
        {
            if(action == AppointmentActionsEnum.Accept.ToString())
                return Map(AppointmentActionsEnum.Accept);
            if(action == AppointmentActionsEnum.Cancel.ToString())
                return Map(AppointmentActionsEnum.Cancel);
            if(action == AppointmentActionsEnum.Refuse.ToString())
                return Map(AppointmentActionsEnum.Refuse);
            if(action == AppointmentActionsEnum.Request.ToString())
                return Map(AppointmentActionsEnum.Request);
            if(action == AppointmentActionsEnum.Report.ToString())
                return Map(AppointmentActionsEnum.Report);
            if(action == AppointmentActionsEnum.Visit.ToString())
                return Map(AppointmentActionsEnum.Visit);
            return "";
        }
    }
}
