namespace Abwaab.Application.Features.Properties.Common
{
    public class AttributeDTO
    {
        public Guid AttributeId { get; set; }
        public string AttributeName { get; set; }
        public Guid DataTypeId { get; set; }
        public string DatayTypeDescription { get; set; }
        public List<AttributePossibleValueDTO>? PossibleValues { get; set; }
    } 
}
