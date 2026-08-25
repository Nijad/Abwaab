using MediatR;

namespace Abwaab.Application.Features.Medias.DeleteMedia
{
    public class DeleteMediaCommand : IRequest<DeleteMediaResponse>
    {
        public Guid MediaId { get; set; }
    }
    public class DeleteMediaResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
