namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class PropertyAttribute: BaseEntity
    {
        public Property Property { get; set; } = null!;
        public Guid PropertyId { get; set; }
        public Attribute Attribute { get; set; } = null!;
        public Guid AttributeId { get; set; }
        public string AttributeValue { get; set; } = null!;
    }
}
