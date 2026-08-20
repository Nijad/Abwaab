namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class AttributeForUpdate
    {
        public Guid AttributeId { get; set; }
        public string AttributeName { get; set; }
        public Guid DataTypeId { get; set; }
        public string DatayTypeDescription { get; set; }
        public List<AttributePossibleValuForUpdate>? PossibleValues { get; set; }
    } 
}
