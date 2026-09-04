using Abwaab.Application.Features.Visitors.DTOs;

namespace Abwaab.Application.Features.Visitors.MostViewedPropertis;

public class MostViewedResponse
{
    public int PagesCount { get; set; }
    public List<MostViewed> Properties { get; set; } = new List<MostViewed>();
}
