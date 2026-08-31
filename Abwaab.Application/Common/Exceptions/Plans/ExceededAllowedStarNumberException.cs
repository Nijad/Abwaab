using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class ExceededAllowedStarNumberException(Plan plan, string title ) : UpgradeRequired426Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.ExceededAllowedStarNumber,
            returnToUser: true)
    {
        string msg = plan.MaxStardPropertiesCountAtSameTime > 0 ?
            $"لا يمكنك تمييز المزيد من العقارات، لقد قمت بالفعل بتمييز {plan.MaxStardPropertiesCountAtSameTime} عقار. قم بترقية اشتراكك" :
            $"الخطة الحالية {plan.Name} لا تسمح لك بتمييز العقار. قم بترقية اشتراكك";
        public override string Message => msg;
    }
}
