using Abwaab.Application.Common.Constants;
using Abwaab.Application.Features.Medias.DeleteMedia;
using Abwaab.Application.Features.Medias.UploadMedia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abwaab.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MediaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MediaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(GeneralConstants.MAX_MEDIA_SIZE_ALLOWED_MB * 1024 * 1024)]
        public async Task<IActionResult> UploadMedia([FromForm] UploadMediaRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            // Validate MediaType matches file content
            using var stream = request.File.OpenReadStream();

            var command = new UploadMediaCommand
            {
                FileName = request.File.FileName,
                Size = request.File.Length,
                ContentType = request.File.ContentType,
                Content = stream,
                PropertyId = request.PropertyId,
                MediaTypeId = request.MediaTypeId,
                MediaTypeName = request.MediaTypeName,
                IsCover = request.IsCover
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{mediaId}")]
        public async Task<IActionResult> DeleteMedia(Guid mediaId)
        {
            DeleteMediaResponse result = await _mediator.Send(new DeleteMediaCommand { MediaId = mediaId });
            
            return Ok(result);
        }
    }
}
