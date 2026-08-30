using MediatR;

namespace Abwaab.Application.Features.Properties.Disable
{
    public class DisablePropertyCommand : IRequest<DisablePropertyResponse>
    {
        public Guid PropertyId { get; set; }
    }
}
