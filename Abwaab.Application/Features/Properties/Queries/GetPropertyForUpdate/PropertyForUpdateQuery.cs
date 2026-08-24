using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class PropertyForUpdateQuery: IRequest<PropertyForUpdateResponse>
    {
        public Guid PropertyId { get; set; }
    }
}
