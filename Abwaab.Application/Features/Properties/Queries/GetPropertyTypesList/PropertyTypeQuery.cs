using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyTypesList
{
    public class PropertyTypeQuery : IRequest<List<PropertyTypeResponse>>
    {
    }
}
