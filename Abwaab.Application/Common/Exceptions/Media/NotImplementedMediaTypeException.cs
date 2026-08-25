using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Media
{
    public class NotImplementedMediaTypeException(string mediaTypeName, string title) : NotImplemented501Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.NotImplementedMediaType,
            returnToUser: false)
    {
        string msg = $"نوع الوسائط '{mediaTypeName}' ليس منجزاً بعد";
        public override string Message => msg;
    };
}
