using Abwaab.Domain.Enums;

namespace Abwaab.Application.Common.Mappings
{
    public static class IdentifiersMapping
    {
        public static string Map(IdentifiersEnum identifier)
        {
            return identifier switch
            {
                IdentifiersEnum.Phone_Number => "رقم موبايل",
                IdentifiersEnum.Email => "بريد الكتروني",
                _ => ""
            };
        }

        public static string Map(string identifier)
        {
            if (identifier == IdentifiersEnum.Email.ToString())
                return Map(IdentifiersEnum.Email);
            if (identifier == IdentifiersEnum.Phone_Number.ToString())
                return Map(IdentifiersEnum.Phone_Number);
            return "";
        }
    }
}
