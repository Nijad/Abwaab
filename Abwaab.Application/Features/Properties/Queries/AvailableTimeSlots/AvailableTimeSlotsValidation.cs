using FluentValidation;

namespace Abwaab.Application.Features.Properties.Queries.AvailableTimeSlots;

public class AvailableTimeSlotsValidation : AbstractValidator<AvailableTimeSlotsQuery>
{
    public AvailableTimeSlotsValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("رقم العقار مطلوب");
    }
}