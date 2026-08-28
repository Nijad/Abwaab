using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Properties.States;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.PropertyEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Enable;

public class EnablePropertyCommandHandler : IRequestHandler<EnablePropertyCommand, EnablePropertyResponse>
{
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IPropertyService _propertyService;
    private readonly string errorTitle = ErrorTitle.EnableProperty;

    public EnablePropertyCommandHandler(
        IPropertyStatesService propertyStatesService,
        IPropertyService propertyService)
    {
        _propertyStatesService = propertyStatesService;
        _propertyService = propertyService;
    }

    public async Task<EnablePropertyResponse> Handle(EnablePropertyCommand request, CancellationToken cancellationToken)
    {
        //get property
        Property property = await _propertyService.FindPropertyWithUserAndStateByIdAsync(request.PropertyId, errorTitle);

        //check if current state is disabled
        PropertyState disabledPropertyState = await _propertyStatesService.GetDisabledPropertyStateAsync(errorTitle);

        if (property.PropertyState != disabledPropertyState)
            throw new NotAllowedToSetPropertyAsPublishedException(property.PropertyState.StateName, errorTitle);

        //update property state
        PropertyState publishedPropertyState = await _propertyStatesService.GetPublishedPropertyStateAsync(errorTitle);

        property.PropertyState = publishedPropertyState;
        property.Note = "";
        await _propertyService.UpdatePropertyAsync(property);

        return new EnablePropertyResponse() { Success = true, Message = "تم تفعيل عرض العقار الخاص بك بنجاح." };
    }
}
