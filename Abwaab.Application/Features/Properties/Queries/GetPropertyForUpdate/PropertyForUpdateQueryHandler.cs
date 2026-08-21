using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class PropertyForUpdateQueryHandler : IRequestHandler<PropertyForUpdateQuery, PropertyForUpdateResponse>
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IPropertyFinishingService _propertyFinishingService;
        private readonly IPropertyTimeSlotService _propertyTimeSlotService;
        private readonly IPropertyAttributeService _propertyAttributeService;
        private readonly string errorTitle = ErrorTitle.PropertyQuery;

        public PropertyForUpdateQueryHandler(
            IUserService userService,
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            IPropertyFinishingService propertyFinishingService,
            IPropertyTimeSlotService propertyTimeSlotService,
            IPropertyAttributeService propertyAttributeService)
        {
            _userService = userService;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _propertyFinishingService = propertyFinishingService;
            _propertyTimeSlotService = propertyTimeSlotService;
            _propertyAttributeService = propertyAttributeService;
        }

        public async Task<PropertyForUpdateResponse> Handle(PropertyForUpdateQuery request, CancellationToken cancellationToken)
        {
            // 1.   check if property exist
            Property property = await _propertyService.FindPropertyByIdForUpdateAsync(request.PropertyId, errorTitle);

            // 2.   get current user
            string username = _userService.FindUserNameByContext();
            ApplicationUser? user = await _userService.FindUserByNameAsync(username);
            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            // 3.   check if property belong to user
            if (property.UserPlan.UserId != user.Id)
                throw new ObjectNotBelongToUserException("العقار", errorTitle);

            // 4.    get property types list
            List<PropertyTypeDTO> typesList = await _propertyTypeService.GetProperyTypesListAsync();

            // 5.    get property finishins list
            List<PropertyFinishingDTO> finishingList = await _propertyFinishingService.GetPropertyFinishingListAsync();

            // 6.    get property time slots list
            List<TimeSlotDTO> timeSlotsList = new();
            if (property.TimeSlots != null)
                foreach (var timeSlot in property.TimeSlots)
                    timeSlotsList.Add(new()
                    {
                        TimeSlotId = timeSlot.Id,
                        Day = timeSlot.Day,
                        DayName = WeekDay.GetDayName(timeSlot.Day),
                        StartTime = timeSlot.StartTime,
                        EndTime = timeSlot.EndTime,
                        Notes = timeSlot.Notes
                    });

            // 7.    get property attributes list
            List<PropertyAttributeDTO> propertyAttributes = null!;
            if (property.PropertyAttributes != null)
            {
                propertyAttributes = new();
                foreach (var propertyAttribute in property.PropertyAttributes)
                    propertyAttributes.Add(new()
                    {
                        PropertyAttributeId = propertyAttribute.Id,
                        Value = propertyAttribute.AttributeValue,
                        AttributeId = propertyAttribute.AttributeId,
                        AttributeName = propertyAttribute.Attribute.AttributeName,
                        DataTypeId = propertyAttribute.Attribute.AttributeDataTypeId,
                        DataTypeDescription = propertyAttribute.Attribute.AttributeDataType?.Name
                    });
            }
            // 8.   get attributes
            List<AttributeDTO> attributes = await _propertyAttributeService.GetAttributesListAsync();

            PropertyForUpdateResponse response = new()
            {
                PropertyId = request.PropertyId,
                Title = property.Title,
                Description = property.Description,
                Address = property.Address,
                AreaInSquareMeter = property.AreaInSquareMeter,
                Price = property.Price,
                Latitude = property.Latitude,
                Longitude = property.Longitude,
                PropertyTypeId = property.PropertyTypeId,
                PropertyFinishingId = property.FinishingId,
                PropertyTypesList = typesList,
                PropertyFinishingsList = finishingList,
                TimeSlots = timeSlotsList,
                PropertyAttributesList = propertyAttributes,
                Attributes = attributes,
                IsStar = property.IsStard
            };
            return response;
        }
    }
}
