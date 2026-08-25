namespace Abwaab.Application.Features.Properties.Common.DTOs
{
    public class MediaDTO
    {
        public Guid MediaId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public Guid MediaTypeId { get; set; }
        public string MediaTypeName { get; set; } = string.Empty;
    }
}
