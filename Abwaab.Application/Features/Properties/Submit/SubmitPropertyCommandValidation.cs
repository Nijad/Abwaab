using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Application.Features.Properties.Common.Validations;
using FluentValidation;

namespace Abwaab.Application.Features.Properties.Submit
{
    public class SubmitPropertyCommandValidation : AbstractValidator<SubmitPropertyCommand>
    {
        public SubmitPropertyCommandValidation()
        {
            // ----- Basic Property Validations -----
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("عنوان العقار مطلوب.")
                .MaximumLength(200)
                .WithMessage("عنوان العقار يجب ألا يتجاوز 200 حرف.");

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                .WithMessage("وصف العقار يجب ألا يتجاوز 2000 حرف.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("عنوان العقار مطلوب");

            RuleFor(x => x.AreaInSquareMeter)
                .NotEmpty().WithMessage("مساحة العقار مطلوبة")
                .GreaterThan(0)
                .WithMessage("مساحة العقار يجب أن تكون أكبر تماماً من الصفر.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("سعر العقار مطلوب")
                .GreaterThan(0)
                .WithMessage("سعر العقار يجب أن يكون أكبر تماماً من الصفر.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("قيمة خط العرض يجب ان تكون بين -90 و 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("قيمة خط الطول يجب أن تكون بين -180 و 180.");

            RuleFor(x => x.PropertyTypeId)
                .NotEmpty().WithMessage("رقم نوع العقار مطلوب.");

            RuleFor(x => x.PropertyFinishingId)
                .NotEmpty().WithMessage("حالة الإكساء مطلوبة");

            // ----- Nested List Validations -----
            // 1. Validate each TimeSlot in the list
            RuleForEach(x => x.TimeSlots)
                .SetValidator(new TimeSlotDTOValidator());

            // 2. Validate each PropertyAttribute in the list
            RuleForEach(x => x.PropertyAttributesList)
                .SetValidator(new PropertyAttributeDTOValidator())
                .When(x => x.PropertyAttributesList != null); // Skip if null

            // 3. (Optional) Ensure the list itself is not empty
            RuleFor(x => x.TimeSlots)
                .NotEmpty()
                .WithMessage("قائمة الأوقات المتاحة للزيارة يجب أن تحتوي فترة زمية واحدة على الأقل.");

            RuleFor(x => x.TimeSlots)
                .Must(BeNonOverlapping)
                .WithMessage("لا يمكن أن تتداخل الفترات الزمنية. يرجى التأكد من اختيار نطاقات زمنية فريدة.")
                .When(x => x.TimeSlots != null && x.TimeSlots.Any());
        }

        private bool BeNonOverlapping(List<TimeSlotDTO> timeSlots)
        {
            if (timeSlots == null || timeSlots.Count < 2)
                return true; // No overlap possible

            // Sort by day, then by start time, and check for overlaps
            var sorted = timeSlots
                .OrderBy(t => t.DayNumber)
                .ThenBy(t => t.StartTime)
                .ToList();

            for (int i = 0; i < sorted.Count - 1; i++)
            {
                var current = sorted[i];
                var next = sorted[i + 1];

                // If same day, check if current.EndTime > next.StartTime
                if (current.DayNumber == next.DayNumber && current.EndTime > next.StartTime)
                    return false; // Overlap detected
            }
            return true;
        }
    }
}
