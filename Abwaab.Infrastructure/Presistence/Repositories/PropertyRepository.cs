using Abwaab.Application.Features.Visitors.Search;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Infrastructure.Presistence.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly AppDbContext _context;

    public PropertyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateProperty(Property property)
    {
        _context.Properties.Add(property);
        await _context.SaveChangesAsync();
    }

    public async Task<Property?> FindPropertyByIdAsync(Guid propertyId)
    {
        return await _context.Properties
            .Include(x => x.PropertyState)
            .Include(x => x.UserPlan)
            .ThenInclude(x => x.Plan)
            .Include(p => p.TimeSlots)
            .Include(p => p.PropertyAttributes)
            .Include(p => p.MediaList)
            //.ThenInclude(x=>x.Attribute)
            //.ThenInclude(x=>x.AttributeDataType)
            .Where(x => x.Id == propertyId)
            .FirstOrDefaultAsync();
    }

    public async Task<Property?> FindPropertyByIdForUpdateAsync(Guid propertyId)
    {
        return await _context.Properties
            .Include(p => p.UserPlan)
            .ThenInclude(x => x.Properties)
            .Include(p => p.UserPlan)
            .ThenInclude(x => x.Plan)
            .Include(p => p.PropertyType)
            .Include(p => p.Finishing)
            .Include(p => p.TimeSlots)
            .Include(p => p.PropertyAttributes)
            .ThenInclude(x => x.Attribute)
            .ThenInclude(x => x.AttributeDataType)
            .Include(x => x.MediaList)
            .ThenInclude(x => x.MediaType)
            .Include(x => x.PropertyState)
            .Where(p => p.Id == propertyId)
            .FirstOrDefaultAsync();
    }

    public async Task<Finishing?> FindPropertyFinishingByIdAsync(Guid finishingId)
    {
        return await _context.Finishings.Where(x => x.Id == finishingId).FirstOrDefaultAsync();
    }

    public async Task<PropertyState?> FindPropertyStateByStateNameAsync(string propertyStateName)
    {
        return await _context.PropertyStates.Where(x => x.StateName == propertyStateName).FirstOrDefaultAsync();
    }

    public async Task<PropertyType?> FindPropertyTypeByIdAsync(Guid propertyTypeId)
    {
        return await _context.PropertyTypes.Where(x => x.Id == propertyTypeId).FirstOrDefaultAsync();
    }

    public async Task<Property?> FindPropertyWithUserAndStateByIdAsync(Guid propertyId)
    {
        return await _context.Properties
            .Include(x => x.UserPlan)
            .ThenInclude(x => x.User)
            .Include(x => x.PropertyState)
            .Where(x => x.Id == propertyId)
            .FirstOrDefaultAsync();
    }

    public async Task<decimal> GetMaxAreaAsync()
    {
        return await _context.Properties.MaxAsync(x => x.AreaInSquareMeter) ?? 0;
    }

    public async Task<decimal> GetMaxPriceAsync()
    {
        return await _context.Properties.MaxAsync(x => x.Price) ?? 0;
    }

    public async Task<decimal> GetMinAreaAsync()
    {
        return await _context.Properties.MinAsync(x => x.AreaInSquareMeter) ?? 0;
    }

    public async Task<decimal> GetMinPriceAsync()
    {
        return await _context.Properties.MinAsync(x => x.Price) ?? 0;
    }
    

    public async Task<List<Property>> GetMostViewedPropertiesAsync(PropertyState publishedProperties, int skip, int take)
    {
        return await _context.Properties
            .Include(x => x.PropertyType)
            .Include(x => x.Finishing)
            .Include(x => x.PropertyAttributes)
            .Include(x => x.MediaList!.Where(y => y.IsCover))
            .Where(x => x.PropertyState == publishedProperties)
            .OrderByDescending(x => x.NumberOfView)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<List<Property>> GetPremiumPropertiesAsync(PropertyState publishedProperties, int skip, int take)
    {

        return await _context.Properties
            .Include(x => x.PropertyType)
            .Include(x => x.Finishing)
            .Include(x => x.PropertyAttributes)
            .Include(x => x.MediaList!.Where(y => y.IsCover))
            .Where(x => x.PropertyState == publishedProperties && x.IsStard)
            .OrderByDescending(x => x.PublishedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<List<Property>> GetPropertiesByStateAsync(PropertyState pendingProperties)
    {
        return await _context.Properties
            .Include(x => x.PropertyType)
            .Include(x => x.Finishing)
            .Include(x => x.PropertyAttributes)
            .Include(x => x.MediaList!.Where(y => y.IsCover))
            .Where(x => x.PropertyState == pendingProperties)
            .ToListAsync();
    }

    public async Task<int> GetPropertiesCountBelongToPlanAsync(Guid planId)
    {
        return _context.Properties.Where(x => x.UserPlandId == planId).Count();
    }

    public async Task<List<Finishing>> GetPropertyFinishingListAsync()
    {
        return await _context.Finishings.ToListAsync();
    }

    public async Task<List<PropertyType>> GetProperyTypesList()
    {
        return await _context.PropertyTypes.ToListAsync();
    }

    public async Task<List<Property>> GetRecentlyAddedPropertiesAsync(PropertyState publishedProperties, int skip, int take)
    {
        return await _context.Properties
            .Include(x => x.PropertyType)
            .Include(x => x.Finishing)
            .Include(x => x.PropertyAttributes)
            .Include(x => x.MediaList!.Where(y => y.IsCover))
            .Where(x => x.PropertyState == publishedProperties)
            .OrderByDescending(x => x.PublishedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetStaredPropertyCountInPlanAsync(Guid userPlandId)
    {
        return await _context.Properties
            .Where(x => x.UserPlandId == userPlandId && x.IsStard)
            .CountAsync();
    }

    public async Task<int> GetTotalPremiumPropertiesCountAsync(PropertyState publishedProperties)
    {
        return await _context.Properties.Where(x => x.PropertyState == publishedProperties && x.IsStard)
            .CountAsync();
    }

    public async Task<int> GetTotalPropertiesCountAsync(PropertyState publishedProperties)
    {
        return await _context.Properties.Where(x => x.PropertyState == publishedProperties)
            .CountAsync();
    }

    public async Task<List<Property>> GetUserPropertiesList(Guid userId)
    {
        return await _context.Properties
            .Include(x => x.PropertyType)
            .Include(x => x.Finishing)
            .Include(x => x.MediaList.Where(y => y.IsCover))
            .Include(x => x.PropertyState)
            .Where(x => x.UserPlan.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> PropertyBelongToUser(Guid userId, Guid propertyId)
    {
        Property property = await _context.Properties.Include(x => x.UserPlan).Where(x => x.Id == propertyId).FirstAsync();
        return property.UserPlan.UserId == userId;
    }

    public async Task<List<Property>> SearchPropertiesAsync(SearchQuery request, List<Attribute> viewSides)
    {
        return await _context.Properties
            .Include(x => x.PropertyType)
            .Include(x => x.Finishing)
            .Include(x => x.PropertyAttributes)
            .ThenInclude(x => x.Attribute)
            .ThenInclude(x => x.AttributeDataType)
            .Include(x => x.MediaList!.Where(y => y.IsCover))
            .Where(x =>
                (string.IsNullOrEmpty(request.TextSearch) || (x.Title != null && x.Title.Contains(request.TextSearch))) &&
                (string.IsNullOrEmpty(request.TextSearch) || (x.Description != null && x.Description.Contains(request.TextSearch))) &&
                (!request.MinPrice.HasValue || (x.Price.HasValue && x.Price.Value >= request.MinPrice.Value)) &&
                (!request.MaxPrice.HasValue || (x.Price.HasValue && x.Price.Value <= request.MaxPrice.Value)) &&
                (!request.MinArea.HasValue || (x.AreaInSquareMeter.HasValue && x.AreaInSquareMeter.Value >= request.MinArea.Value)) &&
                (!request.MaxArea.HasValue || (x.AreaInSquareMeter.HasValue && x.AreaInSquareMeter.Value <= request.MaxArea.Value)) &&
                (!request.PropertyType.HasValue || (x.PropertyTypeId.HasValue && x.PropertyTypeId.Value == request.PropertyType.Value)) &&
                (!request.PropertyFinishing.HasValue || (x.FinishingId.HasValue && x.FinishingId.Value == request.PropertyFinishing.Value)) &&
                (viewSides == null || viewSides.Count == 0 || x.PropertyAttributes.Any(pa => pa.AttributeId == viewSides.First().Id))
            )
            .ToListAsync();
    }

    public async Task UpdatePropertyAsync(Property property)
    {
        _context.Properties.Update(property);
        await _context.SaveChangesAsync();
    }
}
