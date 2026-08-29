using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Presistence.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Appointment appointment)
    {
        await _context.AddAsync(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<AppointmentState?> FindAppointmentByStateNameAsync(string stateName)
    {
        return await _context.AppointmentStates.Where(x => x.StateName == stateName).FirstOrDefaultAsync();
    }

    public async Task<int> GetAppointmentsCountByPropertyAndStateAsync(Guid propertyId, Guid stateId)
    {
        return await _context.Appointments.Where(x => x.PropertyId == propertyId && x.AppointmentStateId == stateId).CountAsync();
    }

    public async Task<List<Appointment>> GetCommingAppointments(Guid propertyId, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Where(x => x.Date > DateTime.Now)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Appointment>> GetBookedAppointments(Guid propertyId, DateOnly startDate, DateOnly endDate, AppointmentState[] states, CancellationToken cancellationToken)
    {
        IQueryable<Appointment> appointments = _context.Appointments
            .Where(
            x => x.PropertyId == propertyId &&
            x.Date >= new DateTime(startDate, new TimeOnly(00,00)) &&
            x.Date <= new DateTime(endDate, new TimeOnly(23,59)));

        if(states != null && states.Length > 0)
            appointments = appointments.Where(x => states.Contains(x.AppointmentState));

        return await appointments.ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> FindAppointmentByIdAsync(Guid appointmentId, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Include(x=>x.User)
            .Include(x=>x.Property)
            .ThenInclude(x=>x.UserPlan)
            .Where(x=>x.Id== appointmentId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAppointment(Appointment appointment, CancellationToken cancellationToken)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
