using Abwaab.Application.Common.Enums;
using Abwaab.Application.Features.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;

namespace Abwaab.Application.Features.Commands.UploadMedia
{
    public class UploadMediaCommand : IRequest<MediaResponseDTO>
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public Stream Content { get; set; } = null!;
        public Property Property { get; set; }
        public Guid PropertyId { get; set; }
        public string MediaType { get; set; }
    }
}
