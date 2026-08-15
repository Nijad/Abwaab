using Abwaab.Application.Common.Enums;

namespace Abwaab.Application.Common.Mappings
{
    public static class NotificationStatesMapping
    {
        public static string Map(NotificationStatesEnum state)
        {
            return state switch
            {
                NotificationStatesEnum.Pending => "بالانتظار",
                NotificationStatesEnum.Sent => "مرسل",
                NotificationStatesEnum.Failed => "فاشل",
                NotificationStatesEnum.Unread => "غير مقروء",
                NotificationStatesEnum.Read => "مقروء",
                _ => ""
            };
        }
    }
}
