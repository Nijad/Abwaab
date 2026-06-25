using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Domain.Entities.MediaEntities
{
    public class Media : BaseEntity
    {
        public string StoragePath { get; set; } = null!;
        public MediaType MediaType { get; set; } = null!;
        public Guid MediaTypeId { get; set; }
        public Advertisment? Advertisment { get; set; }
        public Guid? AdvertismentId { get; set; }
        public Property? Property { get; set; }
        public Guid? PropertyId { get; set; }
        public bool IsDeleted { get; set; }
    }
}