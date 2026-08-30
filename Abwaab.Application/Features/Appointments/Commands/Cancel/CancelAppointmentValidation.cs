using FluentValidation;

namespace Abwaab.Application.Features.Appointments.Commands.Cancel;

public class CancelAppointmentValidation : AbstractValidator<CancelAppointmentCommand>
{
    public CancelAppointmentValidation()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("رقم الموعد مطلوب");
    }
}
