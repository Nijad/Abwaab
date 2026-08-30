using Abwaab.Application.Features.Properties.Common.DTOs;
using FluentValidation;

namespace Abwaab.Application.Features.Properties.Common.Validations
{
    public class TimeSlotDTOValidator : AbstractValidator<TimeSlotDTO>
    {
        public TimeSlotDTOValidator()
        {
            RuleFor(x => x.DayNumber)
                .InclusiveBetween(0, 6) 
                .WithMessage("اليوم يجب أن يكون رقم بين 0 و 6.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("وقت البداية مطلوب.");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("وقت الانتهاء مطلوب.");

            // Ensure StartTime is before EndTime
            RuleFor(x => x)
                .Must(x => x.StartTime < x.EndTime)
                .WithMessage("وقت البداية يجب أن يكون قبل وقت النهاية.");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("الملاحظات يجب ألا تتجاوز 500 حرف.");
        }
    }
}
