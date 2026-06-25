namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class PropertyState: BaseEntity
    {
        public string StateName { get; set; } = null!;
        public List<Property>? Properties { get; set; }
    }
}