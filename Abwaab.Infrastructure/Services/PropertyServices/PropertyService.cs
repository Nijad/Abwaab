using Abwaab.Application.Common.Exceptions.Properties.Attributes;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Queries.UserProperties;
using Abwaab.Application.Features.Visitors.DTOs;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Infrastructure.Services.PropertyServices;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;

    public PropertyService(
        IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<Guid> CreatePropertyAsync(UserPlan userPlan, PropertyState propertyState, string errorTitle)
    {
        Guid id = Guid.NewGuid();

        Property property = new()
        {
            Id = id,
            UserPlan = userPlan,
            PropertyState = propertyState
        };

        await _propertyRepository.CreateProperty(property);
        return id;
    }

    public async Task<bool> HasBalanceToAddPropertyAsync(UserPlan userPlan)
    {
        int propertyCount = await _propertyRepository.GetPropertiesCountBelongToPlanAsync(userPlan.Id);

        return propertyCount < userPlan.Plan.MaxPropertiesCountAtSameTime;
    }

    public async Task<Property> FindPropertyByIdAsync(Guid propertyId, string errorTitle)
    {
        Property? property = await _propertyRepository.FindPropertyByIdAsync(propertyId);

        if (property == null)
            throw new PropertyNotFoundException(errorTitle);

        return property;
    }

    public async Task<bool> PropertyBelongToUser(Guid userId, Guid propertyId)
    {
        return await _propertyRepository.PropertyBelongToUser(userId, propertyId);
    }

    public async Task UpdatePropertyAsync(Property property)
    {
        await _propertyRepository.UpdatePropertyAsync(property);
    }

    public async Task<Property> FindPropertyByIdForUpdateAsync(Guid propertyId, string errorTitle)
    {
        Property? property = await _propertyRepository.FindPropertyByIdForUpdateAsync(propertyId);

        if (property == null)
            throw new PropertyNotFoundException(errorTitle);

        return property;
    }

    public async Task<int> GetStaredPropertyCountInPlanAsync(Guid userPlandId)
    {
        return await _propertyRepository.GetStaredPropertyCountInPlanAsync(userPlandId);
    }

    public async Task<List<UserPropertiesResponse>> GetUserPropertiesSummaryAsync(Guid userId)
    {
        List<Property> propertyList = await _propertyRepository.GetUserPropertiesList(userId);

        List<UserPropertiesResponse> response = new();
        foreach (var property in propertyList)
            response.Add(new()
            {
                propertyId = property.Id,
                AreaInSquareMeter = property.AreaInSquareMeter,
                CoverImage = property.MediaList?.FirstOrDefault()?.FilePath,
                Price = property.Price,
                PropertyFinishing = property.Finishing?.FinishingName,
                PropertyType = property.PropertyType?.TypeName,
                PropertyState = property.PropertyState.StateName,
                Title = property.Title!
            });

        return response;
    }

    public async Task<Property> FindPropertyWithUserAndStateByIdAsync(Guid propertyId, string errorTitle)
    {
        Property? property = await _propertyRepository.FindPropertyWithUserAndStateByIdAsync(propertyId);
        if (property == null)
            throw new PropertyNotFoundException(errorTitle);
        return property;
    }

    public async Task<List<RecentlyAdded>> GetRecentlyAddedPropertiesAsync(PropertyState publishedProperties, List<Attribute> viewSides, int skip, int take)
    {
        List<Property> properties = await _propertyRepository.GetRecentlyAddedPropertiesAsync(publishedProperties, skip, take);

        var recentlyAdded = properties.Select(p => new RecentlyAdded
        {
            PropertyId = p.Id,
            Area = p.AreaInSquareMeter.ToString()!,
            CoverImage = p.MediaList?.FirstOrDefault()?.FilePath!,
            Price = p.Price.ToString()!,
            PropertyFinishing = p.Finishing?.FinishingName!,
            PropertyType = p.PropertyType?.TypeName!,
            Title = p.Title!,
            Address = p.Address!,
            Description = p.Description!,
            ViewSidesList = viewSides.Where(s => p.PropertyAttributes.Select(x => x.AttributeId).Contains(s.Id)).Select(x => x.AttributeName).ToList()
        }).ToList();
        
        return recentlyAdded;
    }

    public async Task<List<Premium>> GetPremiumPropertiesAsync(PropertyState publishedProperties, List<Attribute> viewSides, int skip, int take)
    {
        List<Property> properties = await _propertyRepository.GetPremiumPropertiesAsync(publishedProperties, skip, take);

        var premiumProperties = properties.Select(p => new Premium
        {
            PropertyId = p.Id,
            Area = p.AreaInSquareMeter.ToString()!,
            CoverImage = p.MediaList?.FirstOrDefault()?.FilePath!,
            Price = p.Price.ToString()!,
            PropertyFinishing = p.Finishing?.FinishingName!,
            PropertyType = p.PropertyType?.TypeName!,
            Title = p.Title!,
            Address = p.Address!,
            Description = p.Description!,
            ViewSidesList = viewSides.Where(s => p.PropertyAttributes.Select(x => x.AttributeId).Contains(s.Id)).Select(x => x.AttributeName).ToList()
        }).ToList();

        return premiumProperties;
    }

    public async Task<List<MostViewed>> GetMostViewedPropertiesAsync(PropertyState publishedProperties, List<Attribute> viewSides, int skip, int take)
    {
        List<Property> properties = await _propertyRepository.GetMostViewedPropertiesAsync(publishedProperties, skip, take);

        var mostViewed = properties.Select(p => new MostViewed
        {
            PropertyId = p.Id,
            Area = p.AreaInSquareMeter.ToString()!,
            CoverImage = p.MediaList?.FirstOrDefault()?.FilePath!,
            Price = p.Price.ToString()!,
            PropertyFinishing = p.Finishing?.FinishingName!,
            PropertyType = p.PropertyType?.TypeName!,
            Title = p.Title!,
            Address = p.Address!,
            Description = p.Description!,
            ViewSidesList = viewSides.Where(s => p.PropertyAttributes.Select(x => x.AttributeId).Contains(s.Id)).Select(x => x.AttributeName).ToList()
        }).ToList();

        return mostViewed;
    }

    public async Task<int> GetTotalPropertiesCountAsync(PropertyState publishedProperties)
    {
        return await _propertyRepository.GetTotalPropertiesCountAsync(publishedProperties);
    }

    public async Task<int> GetTotalPremiumPropertiesCountAsync(PropertyState publishedProperties)
    {
        return await _propertyRepository.GetTotalPremiumPropertiesCountAsync(publishedProperties);
    }
}
