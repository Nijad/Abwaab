using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class PropertyAttributeNotBolongToPropertyException(string title) :
        Forbidden403Exception(
            message: "",
            title: title,
            errorCode: ErrorCodes.PropertyAttributeNotBolongToProperty,
            returnToUser: true)
    {
        string msg = $"'ميزة العقار التي تطلبها لا تنتمي إلى هذا العقار";
        public override string Message => msg;
        public string Title { get; set; } = title;
    }
}
