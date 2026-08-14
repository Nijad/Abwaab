using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions
{
    public class NotFoundException(
        string entity,
            string property,
            string value,
            string title,
            string errorCode = "",
            bool returnToUser = false) : 
        CusotomException(
            message: "",
            title: title,
            errorCode: errorCode,
            returnToUser: returnToUser)
    {
        string msg = $"الكيان '{entity}' المعرف بالخاصية '{property}' والتي قيمتها تساوي '{value}' غير موجودة";
        public override string Message => msg;
    }
}
