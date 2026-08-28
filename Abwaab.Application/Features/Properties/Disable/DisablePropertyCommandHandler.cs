using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Properties.States;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;


namespace Abwaab.Application.Features.Properties.Disable;

public class DisablePropertyCommandHandler : IRequestHandler<DisablePropertyCommand, DisablePropertyResponse>
{
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IPropertyService _propertyService;
    private readonly string errorTitle = ErrorTitle.DisableProperty;

    public DisablePropertyCommandHandler(
        IPropertyStatesService propertyStatesService,
        IPropertyService propertyService)
    {
        _propertyStatesService = propertyStatesService;
        _propertyService = propertyService;
    }

    public async Task<DisablePropertyResponse> Handle(DisablePropertyCommand request, CancellationToken cancellationToken)
    {
        //get property
        Property property = await _propertyService.FindPropertyWithUserAndStateByIdAsync(request.PropertyId, errorTitle);

        //check if current state is published
        PropertyState publishedPropertyState = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        if (property.PropertyState != publishedPropertyState)
            throw new NotAllowedToSetPropertyAsDisabledException(property.PropertyState.StateName, errorTitle);

        PropertyState disabledPropertyState = await _propertyStatesService.GetDisabledPropertyStateAsync(errorTitle);
        //update property state

        property.PropertyState = disabledPropertyState;
        property.Note = "";
        await _propertyService.UpdatePropertyAsync(property);

        return new DisablePropertyResponse() { Success = true, Message = "تم إلغاء تفعيل عرض العقار بنجاح." };
    }
}
