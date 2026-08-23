namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class AttributeDataType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<Attribute>? Attributes { get; set; }
    }
}
