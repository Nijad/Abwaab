using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Abwaab.Infrastructure.Presistence.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Presistence.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentState?> FindAppointmentByStateNameAsync(string stateName)
        {
            return await _context.AppointmentStates.Where(x => x.StateName == stateName).FirstOrDefaultAsync();
        }

        public async Task<int> GetAppointmentsCountByPropertyAndStateAsync(Guid propertyId, Guid stateId)
        {
            return await _context.Appointments.Where(x => x.PropertyId == propertyId && x.AppointmentStateId == stateId).CountAsync();
        }
    }
}
