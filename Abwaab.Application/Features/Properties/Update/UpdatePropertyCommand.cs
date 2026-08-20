using MediatR;

namespace Abwaab.Application.Features.Properties.Update
{
    public class UpdatePropertyCommand : IRequest<UpdatePropertyResponse>
    {
        public Guid PropertyId { get; set; }
        public Guid PropertyTypeId { get; set; }
        public Guid FinishingId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal AreaInSquareMeter { get; set; }
        public decimal Price { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
