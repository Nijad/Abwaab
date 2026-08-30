using FluentValidation;

namespace Abwaab.Application.Features.Appointments.Commands.Confirm;

public class ConfirmAppointmentValidation : AbstractValidator<ConfirmAppointmentCommand>
{
    public ConfirmAppointmentValidation()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("رقم الموعد مطلوب");
    }
}