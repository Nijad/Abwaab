using MediatR;

namespace Abwaab.Application.Features.Properties.Sell;

public class SellPropertyCommand :IRequest<SellPropertyResponse>
{
    public Guid PropertyId { get; set; }
}
