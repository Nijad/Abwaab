namespace Abwaab.Application.Common.Exceptions
{
    public class NotFoundException(
        string entity, 
        string property, 
        string id, 
        string errorCode = "", 
        string title ="") : 
            Exception($"The {entity} with the {property}: '{id}' was not found.")
    {
        public  string Title { get; set; } = title;
        public string ErrorCode { get; } = errorCode;
    }
}
