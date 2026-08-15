using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions
{
    public class ObjectNotBelongToUserException(string objectType, string title) :
        Forbidden403Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.ObjectNotBelongToUser,
            returnToUser: true)
    {
        string msg = $"'{objectType}' الذي تحاول تعديله لا ينتمي إليك";
        public override string Message => msg;
        public string Title { get; set; } = title;
    }
}
