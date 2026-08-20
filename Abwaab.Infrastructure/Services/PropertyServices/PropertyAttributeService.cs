using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate;
using Abwaab.Application.Repositories;
using Attribute = Abwaab.Domain.Entities.PropertyEntities.Attribute;

namespace Abwaab.Infrastructure.Services.PropertyServices
{
    public class PropertyAttributeService : IPropertyAttributeService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyAttributeService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<List<AttributeForUpdate>> GetAttributesListAsync()
        {
            List<Attribute> attributes = await _propertyRepository.GetAttributesListAsync();

            List<AttributeForUpdate> list = new();
            foreach (var attribute in attributes)
            {
                List<AttributePossibleValuForUpdate> apvl = null!;
                if (attribute.PossibleValues != null)
                {
                    apvl = new();
                    foreach (var possibleValue in attribute.PossibleValues)
                        apvl.Add(new()
                        {
                            PossibleValueId = possibleValue.Id,
                            PossibleValueDescription = possibleValue.Value
                        });
                }

                list.Add(new()
                {
                    AttributeId = attribute.Id,
                    AttributeName = attribute.AttributeName,
                    DataTypeId = attribute.AttributeDataTypeId,
                    DatayTypeDescription = attribute.AttributeDataType!.Name,
                    PossibleValues = apvl
                });
            }

            return list;
        }
    }
}
