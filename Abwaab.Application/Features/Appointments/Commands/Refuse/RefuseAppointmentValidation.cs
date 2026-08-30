using FluentValidation;

namespace Abwaab.Application.Features.Appointments.Commands.Refuse;

public class RefuseAppointmentValidation : AbstractValidator<RefuseAppointmentCommand>
{
    public RefuseAppointmentValidation()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("رقم الموعد مطلوب");
    }
}
