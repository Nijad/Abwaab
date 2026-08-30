using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions.Appointments;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.AppointmentEntities;

namespace Abwaab.Infrastructure.Services.PropertyServices;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task AddAppointmentAsync(Appointment appointment)
    {
        await _appointmentRepository.AddAsync(appointment);
    }

    public async Task<AppointmentState> FindAppointmentByStateNameAsync(string stateName, string errorTitle)
    {
        AppointmentState? appointmentState = await _appointmentRepository.FindAppointmentByStateNameAsync(stateName);

        if (appointmentState == null)
            throw new AppointmentStateNotFoundException(errorTitle);

        return appointmentState;
    }

    public async Task<AppointmentState> GetConfirmedAppointmentStateAsync(string errorTitle)
    {
        return await FindAppointmentByStateNameAsync(AppointmentStatesEnum.Accepted.ToString(), errorTitle);
    }

    public async Task<int> GetAppointmentsCountByPropertyAndStateAsync(Guid propertyId, Guid stateId)
    {
        return await _appointmentRepository.GetAppointmentsCountByPropertyAndStateAsync(propertyId, stateId);
    }

    public async Task<AppointmentState> GetCanceledAppointmentStateAsync(string errorTitle)
    {
        return await FindAppointmentByStateNameAsync(AppointmentStatesEnum.Canceled.ToString(), errorTitle);
    }

    public async Task<List<Appointment>> GetBookedAppointments(Guid propertyId, DateOnly startDate, DateOnly endDate, string errorTitle, CancellationToken cancellationToken)
    {
        AppointmentState[] states =
        {
            await GetPendingAppointmentStateAsync(errorTitle),
            await GetConfirmedAppointmentStateAsync(errorTitle)
        };
        List<Appointment> commingAppointment = await _appointmentRepository.GetBookedAppointments(propertyId, startDate, endDate, states, cancellationToken);

        return commingAppointment;
    }

    public async Task<AppointmentState> GetCompletedAppointmentStateAsync(string errorTitle)
    {
        return await FindAppointmentByStateNameAsync(AppointmentStatesEnum.Completed.ToString(), errorTitle);
    }

    public async Task<AppointmentState> GetPendingAppointmentStateAsync(string errorTitle)
    {
        return await FindAppointmentByStateNameAsync(AppointmentStatesEnum.Pending.ToString(), errorTitle);
    }

    public async Task<AppointmentState> GetRefusedAppointmentStateAsync(string errorTitle)
    {
        return await FindAppointmentByStateNameAsync(AppointmentStatesEnum.Refused.ToString(), errorTitle);
    }

    public async Task<AppointmentState> GetUnfinishedAppointmentStateAsync(string errorTitle)
    {
        return await FindAppointmentByStateNameAsync(AppointmentStatesEnum.Unfinished.ToString(), errorTitle);
    }

    public async Task<Appointment> FindAppointmentByIdAsync(Guid appointmentId, string errorTitle, CancellationToken cancellationToken)
    {
        Appointment? appointment = await _appointmentRepository.FindAppointmentByIdAsync(appointmentId, cancellationToken);
        if (appointment == null)
            throw new AppointmentNotFoundException(errorTitle);

        return appointment;
    }

    public async Task UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        await _appointmentRepository.UpdateAppointment(appointment, cancellationToken);
    }
}
