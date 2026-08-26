using MediatR;

namespace Abwaab.Application.Features.Properties.Unstar
{
    public class UnstarPropertyCommand:IRequest<UnstarPropertyResponse>
    {
        public Guid PropertyId { get; set; }
    }
}
