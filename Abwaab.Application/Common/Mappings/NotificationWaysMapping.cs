using Abwaab.Domain.Enums;

namespace Abwaab.Application.Common.Mappings
{
    public static class NotificationWaysMapping
    {
        public static string Map(NotificationWaysEnum way)
        {
            return way switch
            {
                NotificationWaysEnum.Web_Application => "تطبيق الانترنت",
                NotificationWaysEnum.SMS => "رسائل قصيرة",
                NotificationWaysEnum.Email => "بريد الكتروني",
                _ => ""
            };
        }

        public static string Map(string way)
        {
            if (way == NotificationWaysEnum.Web_Application.ToString())
                return Map(NotificationWaysEnum.Web_Application);
            if (way == NotificationWaysEnum.SMS.ToString())
                return Map(NotificationWaysEnum.SMS);
            if (way == NotificationWaysEnum.Email.ToString())
                return Map(NotificationWaysEnum.Email);
            return "";
        }
    }
}
