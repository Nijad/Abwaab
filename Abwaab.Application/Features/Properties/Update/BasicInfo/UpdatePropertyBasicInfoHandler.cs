using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Update.BasicInfo
{
    public class UpdatePropertyBasicInfoHandler : IRequestHandler<UpdatePropertyBasicInfoCommand, UpdatePropertyBasicInfoResponse>
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyStatesService _propertyStatesService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IPropertyFinishingService _propertyFinishingService;
        private readonly string errorTitle = ErrorTitle.UpdateProperty;

        public UpdatePropertyBasicInfoHandler(
            IUserService userService,
            IPropertyService propertyService,
            IPropertyStatesService propertyStatesService,
            IPropertyTypeService propertyPropertyTypeService,
            IPropertyFinishingService propertyFinishingService)
        {
            _userService = userService;
            _propertyService = propertyService;
            _propertyStatesService = propertyStatesService;
            _propertyTypeService = propertyPropertyTypeService;
            _propertyFinishingService = propertyFinishingService;
        }

        public async Task<UpdatePropertyBasicInfoResponse> Handle(UpdatePropertyBasicInfoCommand request, CancellationToken cancellationToken)
        {
            //check if property exist
            Property property = await _propertyService.FindPropertyByIdAsync(request.PropertyId, errorTitle);

            //check if property type exist
            PropertyType propertyType = await _propertyTypeService.FindPropertyTypeByIdAsync(request.PropertyTypeId, errorTitle);

            //check if property finishing exist
            Finishing finishing = await _propertyFinishingService.FindPropertyFinishingByIdAsycn(request.FinishingId, errorTitle);

            //check if property belong to user
            string username = _userService.FindUserNameByContext();
            ApplicationUser? user = await _userService.FindUserByNameAsync(username);

            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            //check if property belong to user
            if (property.UserPlan.UserId != user.Id)
                throw new ObjectNotBelongToUserException("العقار", errorTitle);

            //check current state and if need to change
            PropertyState propertyState = await _propertyStatesService.GetNewState(property.PropertyState, errorTitle);

            //reflect changes
            property.Title = request.Title;
            property.Description = request.Description;
            property.Price = request.Price;
            property.Address = request.Address;
            property.Longitude = request.Longitude;
            property.Latitude = request.Latitude;
            property.AreaInSquareMeter = request.AreaInSquareMeter;
            property.FinishingId = request.FinishingId;
            property.PropertyTypeId = request.PropertyTypeId;
            property.PropertyStateId = propertyState.Id;

            //update and save
            await _propertyService.UpdatePropertyAsync(property);

            return new UpdatePropertyBasicInfoResponse() { Success = true, Message = $"تم تعديل العقار بنجاح. حالة العقار الآن هي '{PropertySTatesMapping.Map(propertyState.StateName)}'" };
        }
    }
}
