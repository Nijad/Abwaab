using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Media
{
    public class MediaNotFoundException : NotFound404Exception
    {
        public MediaNotFoundException(string title) : base(
            message: ErrorMessages.MediaNotFound,
            title: title,
            errorCode: ErrorCodes.MediaNotFound,
            returnToUser: true)
        {
        }
    }
}
