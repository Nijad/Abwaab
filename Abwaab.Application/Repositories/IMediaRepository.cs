using Abwaab.Domain.Entities.MediaEntities;

namespace Abwaab.Application.Repositories
{
    public interface IMediaRepository
    {
        Task AddMedia(Media media, CancellationToken cancellationToken);
        Task<Media?> FindMediaByFilePathAsync(string filePath, CancellationToken cancellationToken);
        Task<Media?> FindMediaByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<MediaType?> FindMediaTypeByTypeAsync(string mediaName);
        Task<int> GetMediaCountByPropertyOfDataTypeAsync(Guid propertyId, Guid mediaTypeId);
        Task<List<MediaType>> GetMediaTypesListAsync();
        Task<bool> HasPropertyCoverAsync(Guid propertyId);
        Task RemoveMediaAsync(Media media, CancellationToken cancellationToken);
        Task UncoverImagesAsync(Guid propertyId);
    }
}
