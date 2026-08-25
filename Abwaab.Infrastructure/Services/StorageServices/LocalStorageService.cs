using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.MediaEntities;
using Microsoft.AspNetCore.Hosting;

namespace Abwaab.Infrastructure.Services.StorageServices
{
    public class LocalStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _env;

        public LocalStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task DeleteMedia(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public string GetFolderPath(string propertyId, string mediaTypeName, string errorTitle)
        {
            string folderPath = Path.Combine("uploads", propertyId, mediaTypeName.ToUpper() + "S");

            return folderPath;
        }

        public string GetPhysicalPath(string folderPath)
        {
            string physicalPath = Path.Combine(_env.WebRootPath, folderPath);

            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            return physicalPath;
        }

        public async Task<string> SaveFileAsync(string folderPath, string fileName, Stream content, string errorTitle, CancellationToken cancellationToken)
        {
            string storedFileName = fileName;

            string fullFilePath = Path.Combine(folderPath, storedFileName);

            using FileStream fileStream = new(fullFilePath, FileMode.Create, FileAccess.Write);

            await content.CopyToAsync(fileStream, cancellationToken);

            return fullFilePath;
        }
    }
}
