using Abwaab.Application.Features.Visitors.MainPage;
using Abwaab.Application.Features.Visitors.MostViewedPropertis;
using Abwaab.Application.Features.Visitors.PremiumProperties;
using Abwaab.Application.Features.Visitors.RecentlyAddedProperties;
using Abwaab.Application.Features.Visitors.Search;
using Abwaab.Application.Features.Visitors.SearchForm;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VisitorController : ControllerBase
{
    private readonly IMediator _mediator;
    public VisitorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetMainPageData")]
    public async Task<IActionResult> GetMainPageData()
    {
        MainPageQuery query = new();
        MainPageResponse response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetMostViewed")]
    public async Task<IActionResult> GetMostViewed(int pageNo)
    {
        MostViewedQuery query = new() { PageNo = pageNo };
        MostViewedResponse response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetPremium")]
    public async Task<IActionResult> GetPremium(int pageNo)
    {
        PremiumQuery query = new() { PageNo = pageNo };
        PremiumResponse response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetRecentlyAdded")]
    public async Task<IActionResult> GetRecentlyAdded(int pageNo)
    {
        RecentlyAddedQuery query = new() { PageNo = pageNo };
        RecentlyAddedResponse response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("GetSearchForm")]
    public async Task<IActionResult> GetSearchForm()
    {
        SearchFormQuery query = new();
        SearchFormResponse response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpPost("Search")]
    public async Task<IActionResult> Search(SearchQuery query)
    {
        List<SearchResponse> response = await _mediator.Send(query);
        return Ok(response);
    }
}
