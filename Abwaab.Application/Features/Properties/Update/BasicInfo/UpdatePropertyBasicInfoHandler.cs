using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Update.BasicInfo
{
    public class UpdatePropertyBasicInfoHandler : IRequestHandler<UpdatePropertyBasicInfoCommand, UpdatePropertyBasicInfoResponse>
    {
        private readonly IPropertyService _propertyService;
        private readonly IUserService _userService;
        private readonly string errorTitle = ErrorTitle.UpdateProperty;

        public UpdatePropertyBasicInfoHandler(
            IPropertyService propertyService,
            IUserService userService)
        {
            _propertyService = propertyService;
            _userService = userService;
        }

        public async Task<UpdatePropertyBasicInfoResponse> Handle(UpdatePropertyBasicInfoCommand request, CancellationToken cancellationToken)
        {
            //check if property exist
            Property property = await _propertyService.FindPropertyByIdAsync(request.PropertyId, errorTitle);

            //check if property belong to user
            string username = _userService.FindUserNameByContext();
            ApplicationUser? user = await _userService.FindUserByNameAsync(username);

            if (user == null)
                throw new UserNotFoundException(username, errorTitle);
            
            //check if property belong to user
            if (property.UserPlan.UserId != user.Id)
                throw new ObjectNotBelongToUserException("العقار", errorTitle);

            //check current state and if need to change
            PropertyState propertyState = await _propertyService.GetNewState(property.PropertyState, errorTitle);
            
            //reflect changes
            property.Title = request.Title;
            property.Description = request.Description;
            property.Price = request.Price;
            property.Address = request.Address;
            property.Longitude = request.Longitude;
            property.Latitude = request.Latitude;
            property.AreaInSquareMeter = request.AreaInSquareMeter;
            property.FinishingId = request.FinishingId;
            property.PropertyTypeId = property.PropertyTypeId;
            property.PropertyStateId = propertyState.Id;

            //update and save
            await _propertyService.UpdatePropertyAsync(property);

            return new UpdatePropertyBasicInfoResponse() { Success = true, Message = $"تم تعديل العقار بنجاح. حالة العقار الآن هي '{PropertySTatesMapping.Map(propertyState.StateName)}'" };
        }
    }
}
