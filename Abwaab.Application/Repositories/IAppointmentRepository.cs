using Abwaab.Domain.Entities.AppointmentEntities;

namespace Abwaab.Application.Repositories
{
    public interface IAppointmentRepository
    {
        Task<AppointmentState?> FindAppointmentByStateNameAsync(string stateName);
        Task<int> GetAppointmentsCountByPropertyAndStateAsync(Guid propertyId, Guid stateId);
    }
}
