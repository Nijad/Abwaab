using Abwaab.Application.Contracts;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Infrastructure.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPlanService _planService;

        public PropertyService(
            IPropertyRepository propertyRepository, 
            IPlanService planService)
        {
            _propertyRepository = propertyRepository;
            _planService = planService;
        }

        public async Task<Guid> CreatePropertyAsync()
        {
            Guid id = Guid.NewGuid();
            await _propertyRepository.CreateProperty(id);
            return id;
        }
    }
}
