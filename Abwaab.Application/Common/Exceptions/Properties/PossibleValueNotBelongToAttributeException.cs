using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class PossibleValueNotBelongToAttributeException(string title) :
        Forbidden403Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.PossibleValueNotBelongToAttribute,
            returnToUser: true)
    {
        string msg = $"القيمة المعرفة لا تنتمي إلى الميزة المختارة.";
        public override string Message => msg;
        public string Title { get; set; } = title;
    }
}
