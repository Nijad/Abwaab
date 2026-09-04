using Abwaab.Application.Features.Visitors.DTOs.MainPage;

namespace Abwaab.Application.Features.Visitors.MainPage;

public class  MainPageResponse
{
    public List<RecentlyAdded> RecentlyAddedList { get; set; }
    public List<Premium> PremiumPropertiesList { get; set; }
    public List<MostViewed> MostViewedList { get; set; }
}
