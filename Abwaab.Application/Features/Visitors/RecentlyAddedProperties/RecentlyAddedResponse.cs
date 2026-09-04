using Abwaab.Application.Features.Visitors.DTOs;

namespace Abwaab.Application.Features.Visitors.RecentlyAddedProperties;

public class RecentlyAddedResponse
{
    public int PagesCount { get; set; }
    public List<RecentlyAdded> RecentlyAddedList { get; set; } = new List<RecentlyAdded>();
}
