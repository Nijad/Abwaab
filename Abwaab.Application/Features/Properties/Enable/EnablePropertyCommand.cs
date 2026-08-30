using MediatR;

namespace Abwaab.Application.Features.Properties.Enable;

public class EnablePropertyCommand : IRequest<EnablePropertyResponse>
{
    public Guid PropertyId { get; set; }
}
