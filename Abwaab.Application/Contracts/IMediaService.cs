using Abwaab.Application.Common.Enums;
using Abwaab.Application.Features.Medias.UploadMedia;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.MediaEntities;

namespace Abwaab.Application.Contracts
{
    public interface IMediaService
    {
        Task<Media> SaveMediaAsync(UploadMediaCommand mediaDto, string folderPath, string fileName, string errorTitle, CancellationToken cancellationToken = default);
        Task DeleteMediaAsync(Media media, CancellationToken cancellationToken = default);
        Task<Media> FindMediaByIdAsync(Guid id, string errorTitle, CancellationToken cancellationToken = default);
        Task<List<MediaTypeDTO>> GetAllMediaTypesListAsync();
        Task<MediaType> FindMediaTypeByTypeAsync(MediaTypesEnum typeName, string errorTitle);
        Task<int> GetMediaCountByPropertyOfDataTypeAsync(Guid propertyId, Guid mediaTypeId);
        Task<bool> HasPropertyCoverAsync(Guid propertyId);
        Task UncoverImagesAsync(Guid propertyId);
    }
}
