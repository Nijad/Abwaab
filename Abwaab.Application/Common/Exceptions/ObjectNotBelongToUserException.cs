using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions
{
    //todo: translate
    public class ObjectNotBelongToUserException(string objectType, string title) :
        CusotomException(
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
