using Abwaab.Application.Common.Constants;
using FluentValidation;

namespace Abwaab.Application.Features.Medias.UploadMedia
{
    public class UploadMediaCommandValidator : AbstractValidator<UploadMediaCommand>
    {
        private static readonly string[] AllowedImageTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };
        private static readonly string[] AllowedVideoTypes = { "video/mp4", "video/webm", "video/quicktime" };

        public UploadMediaCommandValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("اسم الملف مطلوب")
                .MaximumLength(255).WithMessage("اسم الملف يجب ألا يتجاوز 255 حرف");

            RuleFor(x => x.Content)
                .NotNull().WithMessage("امتداد الملف مفقود.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("حجم الملف يجب أن يكون أكبر من الصفر")
                .LessThanOrEqualTo(GeneralConstants.MAX_MEDIA_SIZE_ALLOWED_MB * 1024 * 1024)
                .WithMessage($"حجم الملف يجب ألا يتجاوز {GeneralConstants.MAX_MEDIA_SIZE_ALLOWED_MB} ميغابايت.");

            RuleFor(x => x.MediaTypeName)
                .NotEmpty().WithMessage("اسم نوع الملف مطلوب.")
                .MaximumLength(50).WithMessage("اسم نوع الملف يجب ألا يتجاوز 50 حرف");

            RuleFor(x => x.MediaTypeId)
                .NotEmpty().WithMessage("رقم تعريف نوع الملف مطلوب");

            // Validate content type based on MediaType
            RuleFor(x => x)
                .Must(x => IsValidContentType(x.MediaTypeName, x.ContentType))
                .WithMessage("نوع الملف غير مدعوم");
        }

        private bool IsValidContentType(string mediaType, string contentType)
        {
            return mediaType.ToLower() switch
            {
                "image" => AllowedImageTypes.Contains(contentType.ToLower()),
                "video" => AllowedVideoTypes.Contains(contentType.ToLower()),
                _ => false
            };
        }
    }
}
