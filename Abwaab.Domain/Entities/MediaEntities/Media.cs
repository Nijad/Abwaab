using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Domain.Entities.MediaEntities
{
    public class Media : BaseEntity
    {
        public string FileName { get; set; } = string.Empty; // Original user filename
        public string StoredFileName { get; set; } = string.Empty; // Unique server filename
        public string FilePath { get; set; } = string.Empty; // Relative path (/uploads/...)
        public string ContentType { get; set; } = string.Empty; // MIME type
        public long Size { get; set; } // File size in bytes
        public MediaType MediaType { get; set; } = null!;
        public Guid MediaTypeId { get; set; }
        public Property? Property { get; set; }
        public Guid? PropertyId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCover { get; set; }
    }
}