namespace Abwaab.Application.Features.Properties.Common
{
    public class PropertyAttributeDTO
    {
        public Guid PropertyAttributeId { get; set; }
        public string? Value { get; set; }
        public Guid AttributeId { get; set; }
        public string? AttributeName { get; set; }
        public Guid DataTypeId { get; set; }
        public string? DataTypeDescription { get; set; }
    }
}
