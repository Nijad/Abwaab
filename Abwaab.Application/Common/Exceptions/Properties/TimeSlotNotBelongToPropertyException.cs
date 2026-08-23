using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class TimeSlotNotBelongToPropertyException(string title) :
        Forbidden403Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.TimeSlotNotBelongToProperty,
            returnToUser: true)
    {
        string msg = $"الفترة الزمنية التي تطلبها لا تنتمي إلى هذا العقار";
        public override string Message => msg;
        public string Title { get; set; } = title;
    }
}
