using Abwaab.Application.Features.Visitors.DTOs.MainPage;

namespace Abwaab.Application.Features.Visitors.Search;

public class SearchResponse
{
    public int PagesCount { get; set; }
    public List<SearchDTO> Properties { get; set; } = new List<SearchDTO>();
}
