namespace Abwaab.Application.Features.Properties.Common.DTOs;

public class PropertyAttributeBaseDTO
{
    public Guid? AttributeId { get; set; }
    public string? AttributeName { get; set; }
    public string? Value { get; set; }
    public string? DataTypeDescription { get; set; }
}
