namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class PropertyType: BaseEntity
    {
        public string TypeName { get; set; } = null!;
        public List<Property>? Properties { get; set; }
    }
}