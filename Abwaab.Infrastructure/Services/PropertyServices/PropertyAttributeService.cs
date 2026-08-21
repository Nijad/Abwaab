using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Infrastructure.Services.Common;
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

        public async Task<List<AttributeDTO>> GetAttributesListAsync()
        {
            List<Attribute> attributes = await _propertyRepository.GetAttributesListAsync();

            List<AttributeDTO> list = new();
            foreach (var attribute in attributes)
            {
                List<AttributePossibleValueDTO> apvl = null!;
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

        public async Task SyncronizePropertyAttributesAsync(
            List<PropertyAttribute>? existingPropertyAttributes,
            List<PropertyAttributeDTO>? commingPropertyAttributes, 
            Guid propertyId)
        {
            await SyncronizingCollection.Sync(
                existingPropertyAttributes, commingPropertyAttributes,
                (existing, comming) => existing.Id == comming.PropertyAttributeId,
                async (existing) => await DeletePropertyAttributeAsync(existing),
                async (existing, comming) => await UpdatePropertyAttributeAsync(existing, comming),
                async (comming) => await AddPropertyAttributeAsync(comming, propertyId));
        }

        private async Task AddPropertyAttributeAsync(PropertyAttributeDTO comming, Guid propertyId)
        {
            PropertyAttribute propertyAttribute = new()
            {
                Id = Guid.NewGuid(),
                PropertyId = propertyId,
                AttributeId = (Guid)comming.AttributeId!,
                AttributeValue = comming.Value!
            };
            
            await _propertyRepository.AddPropertyAttribute(propertyAttribute);
        }

        private async Task UpdatePropertyAttributeAsync(PropertyAttribute existing, PropertyAttributeDTO comming)
        {
            existing.AttributeId = (Guid)comming.AttributeId!;
            existing.AttributeValue = comming.Value!;
            await _propertyRepository.UpdatePropertyAttributeAsync(existing);
        }

        private async Task DeletePropertyAttributeAsync(PropertyAttribute existing)
        {
            await _propertyRepository.DeletePropertyAttributeAsync(existing);
        }
    }
}
