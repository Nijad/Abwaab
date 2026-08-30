using FluentValidation;

namespace Abwaab.Application.Features.Appointments.Commands.Report;

public class ReportAppointmentValidation : AbstractValidator<ReportAppointmentCommand>
{
    public ReportAppointmentValidation()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("رقم الموعد مطلوب");
    }
}
