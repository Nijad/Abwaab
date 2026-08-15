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
    }
}
