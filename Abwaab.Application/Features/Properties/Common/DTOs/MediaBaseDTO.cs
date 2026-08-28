namespace Abwaab.Application.Features.Properties.Common.DTOs;

public class MediaBaseDTO
{
    public Guid MediaId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string MediaTypeName { get; set; } = string.Empty;
    public bool IsCover { get; set; }
}
