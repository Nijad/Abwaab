using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Media;
using Abwaab.Application.Common.Exceptions.Properties.States;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Save
{
    public class SavePropertyCommandHandler : IRequestHandler<SavePropertyCommand, SavePropertyResponse>
    {
        private readonly IUserService _userService;
        private readonly IPropertyStatesService _propertyStatesService;
        private readonly IPropertyService _propertyService;
        private readonly IMediaService _mediaService;
        private readonly string errorTitle = ErrorTitle.SaveProperty;

        public SavePropertyCommandHandler(
            IUserService userService,
            IPropertyStatesService propertyStatesService,
            IPropertyService propertyService,
            IMediaService mediaService)
        {
            _userService = userService;
            _propertyStatesService = propertyStatesService;
            _propertyService = propertyService;
            _mediaService = mediaService;
        }

        public async Task<SavePropertyResponse> Handle(SavePropertyCommand request, CancellationToken cancellationToken)
        {
            //check if property exist
            Property property = await _propertyService.FindPropertyByIdAsync(request.PropertyId, errorTitle);


            //check if property belong to user
            string username = _userService.FindUserNameByContext(errorTitle);
            ApplicationUser? user = await _userService.FindUserByNameAsync(username);

            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            //check if property belong to user
            if (property.UserPlan.UserId != user.Id)
                throw new ObjectNotBelongToUserException("العقار", errorTitle);

            //check if has cover image
            bool hasCover = await _mediaService.HasPropertyCoverAsync(property.Id);
            if (!hasCover)
                throw new HasNoCoverImageException(errorTitle);

            //check property state if is preparing
            PropertyState preparingState = await _propertyStatesService.GetPreparingPropertyStateAsync(errorTitle);

            if(property.PropertyStateId !=  preparingState.Id)
                throw new NotAllowedToSetPropertyAsPendingException(property.PropertyState.StateName, errorTitle);

            //save property (change state to pending)
            PropertyState pendingState = await _propertyStatesService.GetPendingPropertyStateAsync(errorTitle);
            property.PropertyState= pendingState;
            await _propertyService.UpdatePropertyAsync(property);

            //push notification
            //todo: notificaiton admin

            return new SavePropertyResponse() { Success = true, Message = "تم حفظ العقار بنجاح وهو الآن قيد انتظار موافقة الإدارة، سيتم إعلامكم بذلك في غضون 48 ساعة كحد أقصى." };
        }
    }
}
