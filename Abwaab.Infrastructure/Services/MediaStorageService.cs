using Abwaab.Application.Features.DTOs;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services
{
    public class MediaStorageService : IMediaStorageService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<MediaStorageService> _logger;

        public MediaStorageService(AppDbContext context, IWebHostEnvironment env, ILogger<MediaStorageService> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task<Media> SaveMediaAsync(MediaUploadDTO mediaDto, CancellationToken cancellationToken = default)
        {
            // 1. Generate a unique filename (GUID + original extension)
            var extension = Path.GetExtension(mediaDto.FileName);
            var storedFileName = $"{Guid.NewGuid()}{extension}";

            // 2. Build the folder path: /{entityType}/{year}/{month}/{day}/
            var date = DateTime.UtcNow;
            var folderPath = Path.Combine("uploads", mediaDto.MediaType.ToString(), date.Year.ToString(), date.Month.ToString("D2"), date.Day.ToString("D2"));
            var fullFolderPath = Path.Combine(_env.WebRootPath, folderPath);
            var fullFilePath = Path.Combine(fullFolderPath, storedFileName);

            // 3. Create directory if it doesn't exist
            if (!Directory.Exists(fullFolderPath))
                Directory.CreateDirectory(fullFolderPath);

            // 4. Save the file to disk
            using var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write);
            await mediaDto.Content.CopyToAsync(fileStream, cancellationToken);

            // 5. Save metadata to database
            var media = new Media
            {
                Id = Guid.NewGuid(),
                FileName = mediaDto.FileName,
                StoredFileName = storedFileName,
                FilePath = $"/{folderPath}/{storedFileName}".Replace("\\", "/"), // URL-safe path
                ContentType = mediaDto.ContentType,
                Size = mediaDto.Size,
                Property = mediaDto.Property,
                PropertyId = mediaDto.PropertyId,
                CreatedBy = "System", // Or pass from IUserContext
                CreatedAt = DateTime.UtcNow,
                MediaTypeId = mediaDto.MediaTypeId
            };

            _context.Media.Add(media);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Media saved. Id: {MediaId}, Path: {FilePath}", media.Id, media.FilePath);
            return media;
        }

        public async Task<bool> DeleteMediaAsync(string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                // 1. Remove from database
                var media = await _context.Media.FirstOrDefaultAsync(m => m.FilePath == filePath, cancellationToken);
                if (media != null)
                {
                    _context.Media.Remove(media);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // 2. Remove from file system
                var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation("File deleted: {FilePath}", filePath);
                    return true;
                }

                _logger.LogWarning("File not found: {FilePath}", filePath);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting media: {FilePath}", filePath);
                return false;
            }
        }

        public async Task<Media?> GetMediaByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Media.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }
    }
}
