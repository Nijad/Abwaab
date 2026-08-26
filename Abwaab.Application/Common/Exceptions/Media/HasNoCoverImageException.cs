using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Custom;

namespace Abwaab.Application.Common.Exceptions.Media
{
    public class HasNoCoverImageException : PreconditionRequired428Exception
    {
        public HasNoCoverImageException(string title) : base(
            message: ErrorMessages.HasNoCoverImage,
            title: title,
            errorCode: ErrorCodes.HasNoCoverImage,
            returnToUser: true)
        {
        }
    }
}
