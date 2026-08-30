using Abwaab.Domain.Entities.AppointmentEntities;

namespace Abwaab.Application.Contracts.Properties;

public interface IAppointmentService
{
    Task<AppointmentState> GetPendingAppointmentStateAsync(string errorTitle);
    Task<AppointmentState> GetRefusedAppointmentStateAsync(string errorTitle);
    Task<AppointmentState> GetConfirmedAppointmentStateAsync(string errorTitle);
    Task<AppointmentState> GetCanceledAppointmentStateAsync(string errorTitle);
    Task<AppointmentState> GetUnfinishedAppointmentStateAsync(string errorTitle);
    Task<AppointmentState> GetCompletedAppointmentStateAsync(string errorTitle);
    Task<AppointmentState> FindAppointmentByStateNameAsync(string stateName, string errorTitle);
    Task<int> GetAppointmentsCountByPropertyAndStateAsync(Guid propertyId, Guid stateId);
    Task<List<Appointment>> GetBookedAppointments(Guid propertyId, DateOnly startDate, DateOnly endDate, string errorTitle, CancellationToken cancellationToken);
    Task AddAppointmentAsync(Appointment appointment);
    Task<Appointment> FindAppointmentByIdAsync(Guid appointmentId, string errorTitle, CancellationToken cancellationToken);
    Task UpdateAppointment(Appointment appointment, CancellationToken cancellationToken);
}
