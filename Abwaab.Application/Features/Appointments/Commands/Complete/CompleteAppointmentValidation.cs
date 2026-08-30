using FluentValidation;

namespace Abwaab.Application.Features.Appointments.Commands.Complete;

public class CompleteAppointmentValidation : AbstractValidator<CompleteAppointmentCommand>
{
    public CompleteAppointmentValidation()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("رقم الموعد مطلوب");
    }
}
