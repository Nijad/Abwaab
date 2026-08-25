using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;

namespace Abwaab.Application.Features.Medias.UploadMedia
{
    public class UploadMediaCommand : IRequest<MediaResponse>
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public Stream Content { get; set; } = null!;
        public Guid PropertyId { get; set; }
        public Guid MediaTypeId { get; set; }
        public string MediaTypeName { get; set; } = string.Empty;
    }
}
