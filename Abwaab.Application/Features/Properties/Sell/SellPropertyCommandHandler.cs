using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Properties.States;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Sell;

public class SellPropertyCommandHandler : IRequestHandler<SellPropertyCommand, SellPropertyResponse>
{
    private readonly IPropertyService _propertyService;
    private readonly IUserService _userService;
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly string errorTitle = ErrorTitle.SellProperty;
    public SellPropertyCommandHandler(
        IPropertyService propertyService,
        IUserService userService,
        IPropertyStatesService propertyStatesService)
    {
        _propertyService = propertyService;
        _userService = userService;
        _propertyStatesService = propertyStatesService;
    }
    public async Task<SellPropertyResponse> Handle(SellPropertyCommand request, CancellationToken cancellationToken)
    {
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);

        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        Property property = await _propertyService.FindPropertyByIdAsync(request.PropertyId, errorTitle);

        if(property.UserPlan.UserId != user.Id)
            throw new ObjectNotBelongToUserException("العقار", errorTitle);

        PropertyState published = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);
        if(property.PropertyStateId != published.Id)
            throw new NotAllowedToSetPropertyAsSoldException(property.PropertyState.StateName, errorTitle);

        PropertyState sold = await _propertyStatesService.GetSoldPropertyStateAsync(errorTitle);
        
        property.PropertyState = sold;
        await _propertyService.UpdatePropertyAsync(property);

        return new SellPropertyResponse
        {
            Success = true,
            Message = "تم تعيين حالة العقار إلى مباع بنجاح."
        };
    }
}