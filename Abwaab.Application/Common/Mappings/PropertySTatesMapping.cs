using Abwaab.Application.Common.Enums;

namespace Abwaab.Application.Common.Mappings
{
    public static class PropertySTatesMapping
    {
        public static string Map(PropertyStatesEnum state)
        {
            return state switch
            {
                PropertyStatesEnum.Preparing => "قيد التجهيز",
                PropertyStatesEnum.Pending => "بالانتظار",
                PropertyStatesEnum.Published => "منشور",
                PropertyStatesEnum.Rejected => "مرفوض",
                PropertyStatesEnum.Sold => "مباع",
                PropertyStatesEnum.Deleted => "محذوف",
                PropertyStatesEnum.Disabled => "موقف",
                _ => ""
            };
        }

        public static string Map(string state)
        {
            if (state == PropertyStatesEnum.Preparing.ToString())
                return Map(PropertyStatesEnum.Preparing);
            if (state == PropertyStatesEnum.Pending.ToString())
                return Map(PropertyStatesEnum.Pending);
            if (state == PropertyStatesEnum.Published.ToString())
                return Map(PropertyStatesEnum.Published);
            if (state == PropertyStatesEnum.Rejected.ToString())
                return Map(PropertyStatesEnum.Rejected);
            if (state == PropertyStatesEnum.Sold.ToString())
                return Map(PropertyStatesEnum.Sold);
            if (state == PropertyStatesEnum.Deleted.ToString())
                return Map(PropertyStatesEnum.Deleted);
            if (state == PropertyStatesEnum.Disabled.ToString())
                return Map(PropertyStatesEnum.Disabled);
            return "";
        }
    }
}
