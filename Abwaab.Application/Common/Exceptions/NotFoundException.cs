namespace Abwaab.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ErrorCode { get; init; }
        public string Title { get; init; }
        public NotFoundException(
            string entity,
            string property,
            string id,
            string errorCode = "",
            string title = "") :
            base($"The {entity} with the {property}: '{id}' was not found.")
        {
            ErrorCode = errorCode;
            Title = title;
        }
    }
}
