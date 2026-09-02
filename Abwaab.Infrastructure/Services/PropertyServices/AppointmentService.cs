using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions.Appointments;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Appointments.Queries.GetUserAppointments;
using Abwaab.Application.Features.Appointments.Queries.GetUserAppointments.DTOs;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.UserEntities;
using Azure;

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

    public async Task<GetUserAppointmentsResponse> GetUserAppointmentsByUserIdAsync(Guid userId, string errorTitle)
    {
        AppointmentState pending = await GetPendingAppointmentStateAsync(errorTitle);
        AppointmentState confirmed = await GetConfirmedAppointmentStateAsync(errorTitle);

        List<Appointment> appointments = await _appointmentRepository.GetUserAppointmentsByUserIdAsync(userId);

        IEnumerable<IGrouping<DateTime, Appointment>> groupedByDay = appointments.GroupBy(x => x.Date.Date);

        List<AppointmentsGroupDTO> appointmentsGroup = new();

        foreach (IGrouping<DateTime, Appointment> date in groupedByDay)
        {
            AppointmentsGroupDTO day = new();
            day.AppointmentDate = DateOnly.FromDateTime(date.Key);
            day.DayName = DayOfWeekMapping.Map(date.Key.DayOfWeek);
            day.Appointments = new();

            foreach (Appointment appointment in date)
            {
                AppointmentDetailsDTO details = new();
                ApplicationUser user = 
                    appointment.UserId == userId ? 
                    appointment.User : 
                    appointment.Property.UserPlan.User;

                details.AppointmentId = appointment.Id;
                details.FromTime = TimeOnly.FromDateTime(appointment.Date);
                details.EndTime = appointment.EndTime;
                details.AppointmentState = AppointmentStatesMapping.Map(appointment.AppointmentState.StateName);
                details.AppointmentDirection = appointment.UserId == userId ? "requested" : "received";
                details.Cancelable =
                    appointment.AppointmentState == pending ||
                    appointment.AppointmentState == confirmed ||
                    appointment.Date.AddHours(6) > DateTime.Now;
                details.Comments = appointment.UserComments ?? "";

                details.Firstname = user.FirstName ?? "";
                details.Lastname = user.LastName ?? "";
                details.Email = user.Email ?? "";
                details.PhoneNo = user.PhoneNumber ?? "";

                details.PropertyId = appointment.PropertyId;
                details.PropertyTitle = appointment.Property.Title ?? "";
                details.CoverPath = appointment.Property.MediaList?.Where(x => x.IsCover).FirstOrDefault()?.FilePath ?? "";
                details.Address = appointment.Property.Address ?? "";
                details.Area = appointment.Property.AreaInSquareMeter ?? 0;
                details.Price = appointment.Property.Price ?? 0;
                day.Appointments.Add(details);
            }

            appointmentsGroup.Add(day);
        }

        return new()
        {
            ReceivedAppointments = appointmentsGroup.Where(x => x.Appointments.Any(y => y.AppointmentDirection == "received")).OrderBy(x => x.AppointmentDate).ToList(),
            RequestedAppointments = appointmentsGroup.Where(x => x.Appointments.Any(y => y.AppointmentDirection == "requested")).OrderBy(x => x.AppointmentDate).ToList()
        };
    }
}
