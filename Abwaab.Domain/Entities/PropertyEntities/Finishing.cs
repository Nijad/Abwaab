namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class Finishing: BaseEntity
    {
        public string FinishingName { get; set; } = null!;
        public List<Property>? Properties { get; set; }
    }
}