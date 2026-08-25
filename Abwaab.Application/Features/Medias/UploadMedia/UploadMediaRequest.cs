using Microsoft.AspNetCore.Http;

namespace Abwaab.Application.Features.Medias.UploadMedia
{
    public class UploadMediaRequest
    {
        public IFormFile File { get; set; }
        public Guid PropertyId { get; set; }
        public Guid MediaTypeId { get; set; }
        public string MediaTypeName { get; set; }
    }
}
