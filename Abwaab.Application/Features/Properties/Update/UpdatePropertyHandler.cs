using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Update
{
    public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand, UpdatePropertyResponse>
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyStatesService _propertyStatesService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IPropertyFinishingService _propertyFinishingService;
        private readonly IPropertyTimeSlotService _timeSlotService;
        private readonly IPropertyAttributeService _attributeService;
        private readonly ITransactionManager _transactionManager;
        private readonly string errorTitle = ErrorTitle.UpdateProperty;

        public UpdatePropertyHandler(
            IUserService userService,
            IPropertyService propertyService,
            IPropertyStatesService propertyStatesService,
            IPropertyTypeService propertyPropertyTypeService,
            IPropertyFinishingService propertyFinishingService,
            IPropertyTimeSlotService timeSlotService,
            IPropertyAttributeService attributeService,
            ITransactionManager transactionManager)
        {
            _userService = userService;
            _propertyService = propertyService;
            _propertyStatesService = propertyStatesService;
            _propertyTypeService = propertyPropertyTypeService;
            _propertyFinishingService = propertyFinishingService;
            _timeSlotService = timeSlotService;
            _attributeService = attributeService;
            _transactionManager = transactionManager;
        }

        public async Task<UpdatePropertyResponse> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
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


            //check if property type exist
            PropertyType propertyType = null;
            if (request.PropertyTypeId != null && request.PropertyTypeId != Guid.Empty)
                propertyType = await _propertyTypeService.FindPropertyTypeByIdAsync((Guid)request.PropertyTypeId, errorTitle);

            //check if property finishing exist
            Finishing finishing = null;
            if (request.PropertyFinishingId != null && request.PropertyFinishingId != Guid.Empty)
                finishing = await _propertyFinishingService.FindPropertyFinishingByIdAsycn((Guid)request.PropertyFinishingId, errorTitle);

            //check current state and if need to change
            PropertyState propertyState = await _propertyStatesService.GetNewState(property.PropertyState, errorTitle);

            // todo: check attributes here if exist
            // todo: check validation of its value if compatible with its data type

            //reflect changes
            property.Title = request.Title;
            property.Description = request.Description;
            property.Price = request.Price;
            property.Address = request.Address;
            property.Longitude = request.Longitude;
            property.Latitude = request.Latitude;
            property.AreaInSquareMeter = request.AreaInSquareMeter;
            property.FinishingId = request.PropertyFinishingId;
            property.PropertyTypeId = request.PropertyTypeId;
            property.PropertyStateId = propertyState.Id;
            property.IsStard = request.IsStar;

            await _transactionManager.BeginTransactionAsync(cancellationToken);
            try
            {
                //update and save
                await _propertyService.UpdatePropertyAsync(property);
                
                //update property time solots
                await _timeSlotService.SyncronizePropertyTimeSlotsAsync(property.TimeSlots, request.TimeSlots, property.Id);

                //update property attributes
                await _attributeService.SyncronizePropertyAttributesAsync(
                    property.PropertyAttributes,
                    request.PropertyAttributesList, 
                    property.Id);

                await _transactionManager.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _transactionManager.RollbackTransactionAsync(cancellationToken);
                throw;
            }
            return new UpdatePropertyResponse() { Success = true, Message = $"تم تعديل العقار بنجاح. حالة العقار الآن هي '{PropertySTatesMapping.Map(propertyState.StateName)}'" };
        }
    }
}
