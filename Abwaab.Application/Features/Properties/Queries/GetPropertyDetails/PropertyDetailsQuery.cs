using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyDetails;

public class PropertyDetailsQuery : IRequest<PropertyDetailsResponse>
{
    public Guid PropertyId { get; set; }
}
