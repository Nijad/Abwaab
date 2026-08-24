using Abwaab.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Commands.DeleteMedia
{
    public class DeleteMediaCommandHandler : IRequestHandler<DeleteMediaCommand, bool>
    {
        private readonly IMediaStorageService _mediaStorageService;
        private readonly ILogger<DeleteMediaCommandHandler> _logger;

        public DeleteMediaCommandHandler(IMediaStorageService mediaStorageService, ILogger<DeleteMediaCommandHandler> logger)
        {
            _mediaStorageService = mediaStorageService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteMediaCommand request, CancellationToken cancellationToken)
        {
            var media = await _mediaStorageService.GetMediaByIdAsync(request.MediaId, cancellationToken);
            if (media == null)
                return false;

            var deleted = await _mediaStorageService.DeleteMediaAsync(media.FilePath, cancellationToken);
            if (deleted)
                _logger.LogInformation("File {FilePath} deleted successfully.", media.FilePath);

            return deleted;
        }
    }
}
