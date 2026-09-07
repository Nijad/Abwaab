using MediatR;

namespace Abwaab.Application.Features.Properties.Reject
{
    public class RejectPropertyCommand : IRequest<RejectPropertyResponse>
    {
        public Guid PropertyId { get; set; }
        public string? Note { get; set; }
    }
}
