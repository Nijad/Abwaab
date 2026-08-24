using Abwaab.Domain.Entities.MediaEntities;

namespace Abwaab.Application.Features.DTOs
{
    public class MediaResponseDTO
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty; // Full URL for client
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
        public MediaType MediaType { get; set; }
    }
}
