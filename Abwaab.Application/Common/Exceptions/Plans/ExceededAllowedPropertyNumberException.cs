using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Common.Exceptions.Plans
{
    public class ExceededAllowedPropertyNumberException(Plan plan, string title ) : UpgradeRequired426Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.ExceededAllowedNumber,
            returnToUser: true)
    {
        string msg = plan.MaxPropertiesCountAtSameTime > 0 ?
            $"لا يمكنك إضافة المزيد من العقارات، لقد قمت بالفعل بإضافة {plan.MaxPropertiesCountAtSameTime} عقارات. قم بترقية اشتراكك" :
            $"الخطة الحالية {plan.Name} لا تسمح لك بإضافة العقارات. قم بترقية اشتراكك";
        public override string Message => msg;
    }
}
