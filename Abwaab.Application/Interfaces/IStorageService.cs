using Abwaab.Domain.Entities.MediaEntities;

namespace Abwaab.Application.Interfaces
{
    public interface IStorageService
    {
        Task DeleteMedia(string filePath);
        string GetFolderPath(string propertyId, string mediaTypeName, string errorTitle);
        string GetPhysicalPath(string folderPath);
        Task<string> SaveFileAsync(string folderPath, string fileName, Stream content, string errorTitle, CancellationToken cancellationToken);
    }
}
