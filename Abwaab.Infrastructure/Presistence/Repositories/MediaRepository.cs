using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Presistence.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly AppDbContext _context;

        public MediaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMedia(Media media, CancellationToken cancellationToken)
        {
            _context.Media.Add(media);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Media?> FindMediaByFilePathAsync(string filePath, CancellationToken cancellationToken)
        {
            return await _context.Media.FirstOrDefaultAsync(m => m.FilePath == filePath, cancellationToken);
        }

        public async Task<Media?> FindMediaByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Media
                .Include(m=>m.Property)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<MediaType?> FindMediaTypeByTypeAsync(string mediaName)
        {
            return await _context.MediaTypes.Where(x=>x.Name == mediaName).FirstOrDefaultAsync();
        }

        public async Task<int> GetMediaCountByPropertyOfDataTypeAsync(Guid propertyId, Guid mediaTypeId)
        {
            return await _context.Media.Where(m=>m.PropertyId==propertyId && m.MediaTypeId==mediaTypeId).CountAsync();
        }

        public async Task<List<MediaType>> GetMediaTypesListAsync()
        {
            return await _context.MediaTypes.ToListAsync();
        }

        public async Task RemoveMediaAsync(Media media, CancellationToken cancellationToken)
        {
            _context.Media.Remove(media);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
