using Abwaab.Domain.Entities.AppointmentEntities;

namespace Abwaab.Application.Repositories;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
    Task<Appointment?> FindAppointmentByIdAsync(Guid appointmentId, CancellationToken cancellationToken);
    Task<AppointmentState?> FindAppointmentByStateNameAsync(string stateName);
    Task<int> GetAppointmentsCountByPropertyAndStateAsync(Guid propertyId, Guid stateId);
    Task<List<Appointment>> GetBookedAppointments(Guid propertyId, DateOnly startDate, DateOnly endDate, AppointmentState[] states, CancellationToken cancellationToken);
    Task<List<Appointment>> GetUserAppointmentsByUserIdAsync(Guid userId);
    Task UpdateAppointment(Appointment appointment, CancellationToken cancellationToken);
}
