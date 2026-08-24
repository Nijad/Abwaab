using MediatR;

namespace Abwaab.Application.Features.Commands.DeleteMedia
{
    public class DeleteMediaCommand : IRequest<bool>
    {
        public Guid MediaId { get; set; }
    }
}
