using Abwaab.Application.Features.Visitors.DTOs;

namespace Abwaab.Application.Features.Visitors.PremiumProperties;

public class PremiumResponse
{
    public int PagesCount { get; set; }
    public List<Premium> Properties { get; set; } = new List<Premium>();
}
