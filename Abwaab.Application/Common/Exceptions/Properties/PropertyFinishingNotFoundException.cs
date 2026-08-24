using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Properties
{
    public class PropertyFinishingNotFoundException : NotFound404Exception
    {
        public PropertyFinishingNotFoundException(string title) : base(message: ErrorMessages.PropertyFinishingNotFound,
            title: title,
            errorCode: ErrorCodes.PropertyFinishingNotFound,
            returnToUser: true)
        {
        }
    }
}
