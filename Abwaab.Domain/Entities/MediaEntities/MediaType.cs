namespace Abwaab.Domain.Entities.MediaEntities
{
    public class MediaType : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<Media>? MediaList { get; set; }
    }
}
