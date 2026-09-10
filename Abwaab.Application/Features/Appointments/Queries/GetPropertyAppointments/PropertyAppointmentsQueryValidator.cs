using FluentValidation;

namespace Abwaab.Application.Features.Appointments.Queries.GetPropertyAppointments;

public class PropertyAppointmentsQueryValidator : AbstractValidator<PropertyAppointmentsQuery>
{
    public PropertyAppointmentsQueryValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("رقم العقار مطلوب.");
    }
}