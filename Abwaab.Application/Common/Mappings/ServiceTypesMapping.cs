using Abwaab.Application.Common.Enums;

namespace Abwaab.Application.Common.Mappings
{
    public static class ServiceTypesMapping
    {
        public static string Map(ServiceTypesEnum type)
        {
            return type switch
            {
                ServiceTypesEnum.Advertisment => "إعلان",
                ServiceTypesEnum.Plan_Subscription => "اشتراك في خطة",
                _ => ""
            };
        }
    }
}
