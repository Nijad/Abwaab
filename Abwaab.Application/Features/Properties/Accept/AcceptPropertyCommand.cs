using MediatR;

namespace Abwaab.Application.Features.Properties.Accept;

public class AcceptPropertyCommand : IRequest<AcceptPropertyResponse>
{
    public Guid PropertyId { get; set; }
}
