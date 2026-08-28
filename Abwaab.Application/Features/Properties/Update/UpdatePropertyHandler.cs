using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Properties.Attributes;
using Abwaab.Application.Common.Exceptions.Properties.DataTypes;
using Abwaab.Application.Common.Exceptions.Properties.States;
using Abwaab.Application.Common.Exceptions.Properties.TimeSlots;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using System.Text.Json;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

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
            string username = _userService.FindUserNameByContext(errorTitle);
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

            PropertyState preparingState = await _propertyStatesService.GetPreparingPropertyStateAsync(errorTitle);

            if (property.PropertyState != preparingState)
                throw new NotAllowedToSetPropertyAsPreparingException(property.PropertyState.StateName, errorTitle);

            //check time slot
            if (request.TimeSlots != null && request.TimeSlots.Count > 0)
                foreach (var item in request.TimeSlots)
                    if (item.TimeSlotId != null && item.TimeSlotId != Guid.Empty)
                    {
                        TimeSlot timeSlot = await _timeSlotService.FindTimeSlotByIdAsync(item.TimeSlotId, errorTitle);

                        //check if time slot belong to the same property
                        if (timeSlot.PropertyId != property.Id)
                            throw new TimeSlotNotBelongToPropertyException(errorTitle);
                    }

            if (request.PropertyAttributesList != null && request.PropertyAttributesList.Count > 0)
                foreach (var item in request.PropertyAttributesList)
                    if (item.PropertyAttributeId != null && item.PropertyAttributeId != Guid.Empty)
                    {
                        PropertyAttribute? propertyAttribute = await _attributeService.FindPropertyAttributeByIdAsync(item.PropertyAttributeId, errorTitle);

                        if (propertyAttribute.PropertyId != property.Id)
                            throw new PropertyAttributeNotBolongToPropertyException(errorTitle);

                        Attribute attribute = await _attributeService.FindAttributeByIdAsync(item.AttributeId, errorTitle);

                        //check if data type exist
                        AttributeDataType attributeDataType = await _attributeService.FindAttributeDataTypeByIdAsync(attribute.AttributeDataTypeId, errorTitle);

                        if (attributeDataType.Name == AttributeDataTypeEnum.number.ToString())
                        {
                            int o;
                            if (!int.TryParse(item.Value, out o))
                                throw new NotValidNumberException(errorTitle);
                            if (o <= 0)
                                throw new NotValidNumberException(errorTitle);
                        }
                        else if (attributeDataType.Name == AttributeDataTypeEnum.boolean.ToString())
                        {
                            if (item.Value != "0" &&
                                item.Value != "1" &&
                                item.Value.ToLower() != "false" &&
                                item.Value.ToLower() != "true")
                                throw new NotValidBooleanException(errorTitle);
                        }
                        else if (attributeDataType.Name == AttributeDataTypeEnum.list.ToString())
                        {
                            //check if value belong to the list
                            PossibleValueDTO? pvDto;
                            pvDto = JsonSerializer.Deserialize<PossibleValueDTO>(item.Value);

                            if (pvDto.UnmatchedProperties != null)
                                throw new NotValidFormatException(errorTitle);

                            AttributePossibleValue apv = await _attributeService.FindAttributePossibleValueByIdAsync(pvDto.possibleValueId, errorTitle);

                            if (apv.AttributeId != attribute.Id)
                                throw new PossibleValueNotBelongToAttributeException(errorTitle);
                        }
                    }

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
            return new UpdatePropertyResponse() { Success = true, Message = $"تم حفظ تعديلات العقار بنجاح" };
        }
    }
}
