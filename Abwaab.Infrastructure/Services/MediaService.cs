using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Media;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Medias.UploadMedia;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.MediaEntities;

namespace Abwaab.Infrastructure.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;

        public MediaService(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public async Task<Media> SaveMediaAsync(UploadMediaCommand mediaDto, string folderPath, string fileName, string errorTitle, CancellationToken cancellationToken = default)
        {
            var media = new Media
            {
                Id = Guid.NewGuid(),
                FilePath = $"/{folderPath}/{fileName}".Replace("\\", "/"),
                ContentType = mediaDto.ContentType,
                Size = mediaDto.Size,
                PropertyId = mediaDto.PropertyId,
                CreatedBy = "System", // Or pass from IUserContext
                CreatedAt = DateTime.UtcNow,
                MediaTypeId = mediaDto.MediaTypeId,
                IsCover = mediaDto.IsCover
            };

            await _mediaRepository.AddMedia(media, cancellationToken);

            return media;
        }

        public async Task DeleteMediaAsync(Media media, CancellationToken cancellationToken = default)
        {
            await _mediaRepository.RemoveMediaAsync(media, cancellationToken);
        }

        public async Task<Media> FindMediaByIdAsync(Guid id, string errorTitle, CancellationToken cancellationToken = default)
        {
            Media? media = await _mediaRepository.FindMediaByIdAsync(id, cancellationToken);
            if (media == null)
                throw new MediaNotFoundException(errorTitle);

            return media;
        }

        public async Task<List<MediaTypeDTO>> GetAllMediaTypesListAsync()
        {
            var mediaTypes = await _mediaRepository.GetMediaTypesListAsync();
            List<MediaTypeDTO> result = new List<MediaTypeDTO>();

            foreach (var mediaType in mediaTypes)
                result.Add(new() { MediaTypeId = mediaType.Id, MediaTypeName = mediaType.Name });

            return result;
        }

        public async Task<MediaType> FindMediaTypeByTypeAsync(MediaTypesEnum image, string errorTitle)
        {
            MediaType? mediaType = await _mediaRepository.FindMediaTypeByTypeAsync(image.ToString());
            if (mediaType == null)
                throw new NotFoundException(nameof(MediaType), nameof(mediaType.Name), image.ToString(), errorTitle);

            return mediaType;
        }

        public async Task<int> GetMediaCountByPropertyOfDataTypeAsync(Guid propertyId, Guid mediaTypeId)
        {
            return await _mediaRepository.GetMediaCountByPropertyOfDataTypeAsync(propertyId, mediaTypeId);
        }

        public async Task<bool> HasPropertyCoverAsync(Guid propertyId)
        {
            return await _mediaRepository.HasPropertyCoverAsync(propertyId);
        }

        public async Task UncoverImagesAsync(Guid propertyId)
        {
            await _mediaRepository.UncoverImagesAsync(propertyId);
        }
    }
}
