namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class PropertyAttributeForUpdate
    {
        public Guid PropertyAttributeId { get; set; }
        public string? Value { get; set; }
        public Guid AttributeId { get; set; }
        public string? AttributeName { get; set; }
        public Guid DataTypeId { get; set; }
        public string? DataTypeDescription { get; set; }
    }
}
