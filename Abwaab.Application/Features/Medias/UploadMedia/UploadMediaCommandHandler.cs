using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Media;
using Abwaab.Application.Common.Exceptions.Plans;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Medias.UploadMedia
{
    public class UploadMediaCommandHandler : IRequestHandler<UploadMediaCommand, MediaResponse>
    {
        private readonly IMediaService _mediaService;
        private readonly ILogger<UploadMediaCommandHandler> _logger;
        private readonly ITransactionManager _transactionManager;
        private readonly IStorageService _storageService;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly string errorTitle = ErrorTitle.UploadMedia;

        public UploadMediaCommandHandler(
            IMediaService mediaService,
            ILogger<UploadMediaCommandHandler> logger,
            ITransactionManager transactionManager,
            IStorageService storageService,
            IUserService userService,
            IPropertyService propertyService)
        {
            _mediaService = mediaService;
            _logger = logger;
            _transactionManager = transactionManager;
            _storageService = storageService;
            _userService = userService;
            _propertyService = propertyService;
        }

        public async Task<MediaResponse> Handle(UploadMediaCommand command, CancellationToken cancellationToken)
        {
            Media media = null!;
            await _transactionManager.BeginTransactionAsync(cancellationToken);
            try
            {
                //get user
                string username = _userService.FindUserNameByContext(errorTitle);
                ApplicationUser? user = await _userService.FindUserByNameAsync(username);
                if (user == null)
                    throw new UserNotFoundException(username, errorTitle);

                Property property = await _propertyService.FindPropertyByIdAsync(command.PropertyId, errorTitle);

                if (user.Id != property.UserPlan.UserId)
                    throw new ObjectNotBelongToUserException("العقار", errorTitle);

                int mediaCount = await _mediaService.GetMediaCountByPropertyOfDataTypeAsync(command.PropertyId, command.MediaTypeId);

                //check if allowed adding media to property
                bool canUpload;
                MediaType mediaType = await _mediaService.FindMediaTypeByTypeAsync(MediaTypesEnum.Image, errorTitle);

                if (mediaType.Id != command.MediaTypeId)
                {
                    mediaType = await _mediaService.FindMediaTypeByTypeAsync(MediaTypesEnum.Video, errorTitle);
                    if (mediaType.Id != command.MediaTypeId)
                        throw new NotImplementedMediaTypeException(command.MediaTypeName, errorTitle);
                    else
                        canUpload = property.UserPlan.Plan.MaxVideosCount > mediaCount;

                    if (!canUpload)
                        throw new ExceededAllowedImageNumberException(property.UserPlan.Plan, errorTitle);
                }
                else
                {
                    canUpload = property.UserPlan.Plan.MaxImagesCount > mediaCount;

                    if (!canUpload)
                        throw new ExceededAllowedImageNumberException(property.UserPlan.Plan, errorTitle);
                }
                //store media file 
                string extension = Path.GetExtension(command.FileName);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string folderPath = _storageService.GetFolderPath(command.PropertyId.ToString(), command.MediaTypeName, errorTitle);

                string physicalPath = _storageService.GetPhysicalPath(folderPath);

                string filePath = await _storageService.SaveFileAsync(physicalPath, fileName, command.Content, errorTitle, cancellationToken);

                //check if media is cover remove other covers
                if (command.IsCover)
                    await _mediaService.UncoverImagesAsync(property.Id);

                //save media database
                media = await _mediaService.SaveMediaAsync(command, folderPath, fileName, errorTitle, cancellationToken);

                await _transactionManager.CommitTransactionAsync(cancellationToken);
                _logger.LogInformation("File {FileName} uploaded successfully. MediaId: {MediaId}", command.FileName, media.Id);
                return new MediaResponse
                {
                    Success = true,
                    Message = $"لقد تم تحميل {MediaTypesMapping.Map(command.MediaTypeName)} بنجاح",
                    Id = media.Id,
                    FilePath = media.FilePath,
                };
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync(cancellationToken);
                if (media != null)
                    await _storageService.DeleteMedia(media.FilePath);
                throw;
            }
        }
    }
}
