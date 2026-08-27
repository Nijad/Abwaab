namespace Abwaab.Application.Features.Properties.Queries.UserProperties
{
    public class UserPropertiesResponse
    {
        public Guid propertyId { get; set; }
        public string? CoverImage { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string? PropertyType { get; set; } = string.Empty;
        public string? PropertyFinishing { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public decimal? AreaInSquareMeter { get; set; }
        public int? VisitRequest { get; set; }
        public string? PropertyState { get; set; } = string.Empty;
    }
}
