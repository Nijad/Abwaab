using Abwaab.Application.Common.Enums;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Features.DTOs
{
    public class MediaUploadDTO
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public Stream Content { get; set; } = null!; // The actual file stream
        public Property Property { get; set; }
        public Guid PropertyId { get; set; } // Optional FK
        public string MediaType { get; set; }
        public Guid MediaTypeId { get; set; }
    }
}
