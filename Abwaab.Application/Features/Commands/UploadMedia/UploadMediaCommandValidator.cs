using Abwaab.Application.Common.Enums;
using FluentValidation;

namespace Abwaab.Application.Features.Commands.UploadMedia
{
    // Application/Features/Media/Commands/UploadMedia/UploadMediaCommandValidator.cs
    public class UploadMediaCommandValidator : AbstractValidator<UploadMediaCommand>
    {
        private static readonly string[] AllowedImageTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };
        private static readonly string[] AllowedVideoTypes = { "video/mp4", "video/webm", "video/quicktime" };

        public UploadMediaCommandValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Content)
                .NotNull();

            RuleFor(x => x.Size)
                .GreaterThan(0)
                .LessThanOrEqualTo(50 * 1024 * 1024) // 50MB
                .WithMessage("File size cannot exceed 50MB.");

            //RuleFor(x => x.EntityType)
            //    .NotEmpty()
            //    .MaximumLength(50);

            RuleFor(x => x.MediaType)
                .IsInEnum();

            // Validate content type based on MediaType
            RuleFor(x => x)
                .Must(x => IsValidContentType(x.MediaType, x.ContentType))
                .WithMessage("File type is not supported for the specified MediaType.");
        }

        private bool IsValidContentType(string mediaType, string contentType)
        {
            return mediaType switch
            {
                "Image" => AllowedImageTypes.Contains(contentType.ToLower()),
                "Video" => AllowedVideoTypes.Contains(contentType.ToLower()),
                _ => false
            };
        }
    }
}
