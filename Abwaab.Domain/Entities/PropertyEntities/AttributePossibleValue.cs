namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class AttributePossibleValue: BaseEntity
    {
        public string Value { get; set; } = null!;
        public Attribute Attribute { get; set; } = null!;
        public Guid AttributeId { get; set; }
    }
}