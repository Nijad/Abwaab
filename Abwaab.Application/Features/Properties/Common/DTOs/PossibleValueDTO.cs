using System.Text.Json;
using System.Text.Json.Serialization;

namespace Abwaab.Application.Features.Properties.Common.DTOs
{
    public class PossibleValueDTO
    {
        public Guid? possibleValueId { get; set; }
        public string? possibleValueDescription { get; set; }
        
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? UnmatchedProperties { get; set; }
    }
}
