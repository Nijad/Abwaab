using FluentValidation;

namespace Abwaab.Application.Features.Appointments.Book;

public class BookAppointmentValidation : AbstractValidator<BookAppointmentCommand>
{
    public BookAppointmentValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("رقم العقار مطلوب");
        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage("تاريخ الموعد مطلوب")
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("تاريخ الموعد يجب أن يكون في المستقبل.");
        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("وقت نهاية الموعد مطلوب")
            .GreaterThan(x => TimeOnly.FromDateTime(x.AppointmentDate)).WithMessage("وقت نهاية الموقع يجب أن يكون بعد بداية الموعد");
    }
}