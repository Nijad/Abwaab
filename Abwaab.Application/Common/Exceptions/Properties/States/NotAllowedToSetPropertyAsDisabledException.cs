using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;
using Abwaab.Application.Common.Mappings;
namespace Abwaab.Application.Common.Exceptions.Properties.States;

public class NotAllowedToSetPropertyAsDisabledException(string stateName, string title) :
        MethodNotAllowed405Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.NotAllowedToChangePropertyState,
            returnToUser: true)
{
    string msg = $"لا يمكنك وضع العقار بحالة إلغاء العرض، حيث أن حالة العقار الحالية هي '{PropertySTatesMapping.Map(stateName)}'.";
    public override string Message => msg;
}
