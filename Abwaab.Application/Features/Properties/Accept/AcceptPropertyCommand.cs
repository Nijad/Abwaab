using MediatR;

namespace Abwaab.Application.Features.Properties.Accept;

public class DisablePropertyCommand : IRequest<DisablePropertyResponse>
{
    public Guid PropertyId { get; set; }
}
