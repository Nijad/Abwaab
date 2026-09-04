using MediatR;

namespace Abwaab.Application.Features.Visitors.Search;

public class SearchQuery : IRequest<List<SearchResponse>>
{
    public string? TextSearch { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinArea { get; set; }
    public decimal? MaxArea { get; set; }
    public Guid? PropertyType { get; set; }
    public Guid? PropertyFinishing { get; set; }
    public List<Guid>? ViewSides { get; set; }
}
