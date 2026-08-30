using MediatR;

namespace Abwaab.Application.Features.Appointments.Commands.Book;

public class BookAppointmentCommand : IRequest<BookAppointmentResponse>
{
    public Guid PropertyId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeOnly EndTime { get; set; }
}