using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.MediaEntities;
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
        private readonly IPropertyAttributeService _propertyAttributeService;
        private readonly IMediaService _mediaService;
        private readonly string errorTitle = ErrorTitle.PropertyQuery;

        public PropertyForUpdateQueryHandler(
            IUserService userService,
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            IPropertyFinishingService propertyFinishingService,
            IPropertyAttributeService propertyAttributeService,
            IMediaService mediaService)
        {
            _userService = userService;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _propertyFinishingService = propertyFinishingService;
            _propertyAttributeService = propertyAttributeService;
            _mediaService = mediaService;
        }

        public async Task<PropertyForUpdateResponse> Handle(PropertyForUpdateQuery request, CancellationToken cancellationToken)
        {
            // 1.   check if property exist
            Property property = await _propertyService.FindPropertyByIdForUpdateAsync(request.PropertyId, errorTitle);

            // 2.   get current user
            string username = _userService.FindUserNameByContext(errorTitle);
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

            //9.    get remaining stars allowed
            int remainingStars = property.UserPlan.Plan.MaxStardPropertiesCountAtSameTime -
                property.UserPlan.Properties.Where(x => x.IsStard).Count();

            //10.   get remaining images allowed

            MediaType imageMediaType = await _mediaService.FindMediaTypeByTypeAsync(MediaTypesEnum.Image, errorTitle);

            int remainingImages = property.UserPlan.Plan.MaxImagesCount -
                property.MediaList.Where(x => x.MediaType == imageMediaType && !x.IsDeleted).Count();

            //11.   get remaingin videos allowed
            MediaType vedioMediaType = await _mediaService.FindMediaTypeByTypeAsync(MediaTypesEnum.Video, errorTitle);

            int remainingVedios = property.UserPlan.Plan.MaxImagesCount -
                property.MediaList.Where(x => x.MediaType == vedioMediaType && !x.IsDeleted).Count();

            //12.   get media types list
            List<MediaTypeDTO> mediaTypes = await _mediaService.GetAllMediaTypesListAsync();

            //13.   get property media list
            List<MediaDTO> mediaDTOs = new List<MediaDTO>();
            foreach (var media in property.MediaList)
                mediaDTOs.Add(new()
                {
                    MediaId = media.Id,
                    FilePath = media.FilePath,
                    MediaTypeId = media.MediaTypeId,
                    MediaTypeName = media.MediaType.Name,
                    IsCover = media.IsCover,
                });

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
                MediaTypes = mediaTypes,
                PropertyAttributesList = propertyAttributes,
                PropertyMediaList = mediaDTOs,
                Attributes = attributes,
                RemainingStarsAllowed = remainingStars,
                RemainingImagesAllowed = remainingImages,
                RemainingVideosAllowed = remainingVedios,
                PropertyState = property.PropertyState.StateName,
                IsStar = property.IsStard,
            };
            return response;
        }
    }
}
