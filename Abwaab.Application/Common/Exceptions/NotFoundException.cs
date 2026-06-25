namespace Abwaab.Application.Common.Exceptions
{
    public class NotFoundException(string entity, string property, string id) : Exception($"The {entity} with the {property}: {id} was not found.");
}
