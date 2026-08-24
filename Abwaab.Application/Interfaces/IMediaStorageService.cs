using Abwaab.Application.Features.DTOs;
using Abwaab.Domain.Entities.MediaEntities;

namespace Abwaab.Application.Interfaces
{
    public interface IMediaStorageService
    {
        Task<Media> SaveMediaAsync(MediaUploadDTO mediaDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteMediaAsync(string filePath, CancellationToken cancellationToken = default);
        Task<Media?> GetMediaByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
