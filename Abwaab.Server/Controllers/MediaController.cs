using Abwaab.Application.Features.Commands.DeleteMedia;
using Abwaab.Application.Features.Commands.UploadMedia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    // Presentation/Controllers/MediaController.cs
    [ApiController]
    [Route("api/media")]
    [Authorize] // Require authentication
    public class MediaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MediaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(50 * 1024 * 1024)] // 50MB limit
        public async Task<IActionResult> UploadMedia([FromForm] IFormFile file, [FromForm] Guid? propertyId, [FromForm] string mediaType)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Validate MediaType matches file content
            using var stream = file.OpenReadStream();

            var command = new UploadMediaCommand
            {
                FileName = file.FileName,
                Size = file.Length,
                ContentType = file.ContentType,
                Content = stream,
                PropertyId = (Guid)propertyId,
                MediaType = mediaType
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{mediaId}")]
        public async Task<IActionResult> DeleteMedia(Guid mediaId)
        {
            var command = new DeleteMediaCommand { MediaId = mediaId };
            var result = await _mediator.Send(command);
            if (result)
                return Ok(new { Message = "Media deleted successfully." });
            return NotFound(new { Message = "Media not found." });
        }
    }
}
