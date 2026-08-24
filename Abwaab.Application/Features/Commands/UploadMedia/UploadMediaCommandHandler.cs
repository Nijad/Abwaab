using Abwaab.Application.Features.DTOs;
using Abwaab.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Commands.UploadMedia
{
    public class UploadMediaCommandHandler : IRequestHandler<UploadMediaCommand, MediaResponseDTO>
    {
        private readonly IMediaStorageService _mediaStorageService;
        private readonly ILogger<UploadMediaCommandHandler> _logger;

        public UploadMediaCommandHandler(IMediaStorageService mediaStorageService, ILogger<UploadMediaCommandHandler> logger)
        {
            _mediaStorageService = mediaStorageService;
            _logger = logger;
        }

        public async Task<MediaResponseDTO> Handle(UploadMediaCommand request, CancellationToken cancellationToken)
        {
            var mediaDto = new MediaUploadDTO
            {
                FileName = request.FileName,
                Size = request.Size,
                ContentType = request.ContentType,
                Content = request.Content,
                Property = request.Property,
                PropertyId = request.PropertyId,
                MediaType = request.MediaType
            };

            var media = await _mediaStorageService.SaveMediaAsync(mediaDto, cancellationToken);

            _logger.LogInformation("File {FileName} uploaded successfully. MediaId: {MediaId}", request.FileName, media.Id);

            return new MediaResponseDTO
            {
                Id = media.Id,
                FileName = media.FileName,
                FilePath = media.FilePath,
                ContentType = media.ContentType,
                Size = media.Size,
                MediaType = media.MediaType
            };
        }
    }
}
