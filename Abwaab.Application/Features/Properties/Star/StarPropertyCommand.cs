using MediatR;

namespace Abwaab.Application.Features.Properties.Star
{
    public class StarPropertyCommand:IRequest<StarPropertyResponse>
    {
        public Guid PropertyId { get; set; }
    }
}
