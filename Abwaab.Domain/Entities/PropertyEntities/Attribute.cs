using Abwaab.Domain.Enums;

namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class Attribute : BaseEntity
    {
        public string AttributeName { get; set; } = null!;
        public AttributeDataType DataType { get; set; }
        public List<PropertyAttribute>? PropertyAttributes { get; set; }
        public List<AttributePossibleValue>? PossibleValues { get; set; }
    }
}
