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

        public static string Map(string state)
        {
            if(state == NotificationStatesEnum.Pending.ToString())
                return Map(NotificationStatesEnum.Pending);
            if(state == NotificationStatesEnum.Sent.ToString())
                return Map(NotificationStatesEnum.Sent);
            if(state == NotificationStatesEnum.Failed.ToString())
                return Map(NotificationStatesEnum.Failed);
            if(state == NotificationStatesEnum.Unread.ToString())
                return Map(NotificationStatesEnum.Unread);
            if(state == NotificationStatesEnum.Read.ToString())
                return Map(NotificationStatesEnum.Read);
            return "";
        }
    }
}
