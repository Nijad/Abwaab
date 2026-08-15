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
    }
}
