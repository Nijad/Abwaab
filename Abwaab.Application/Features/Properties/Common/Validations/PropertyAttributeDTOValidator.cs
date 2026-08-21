using Abwaab.Application.Features.Properties.Common.DTOs;
using FluentValidation;

namespace Abwaab.Application.Features.Properties.Common.Validations
{
    public class PropertyAttributeDTOValidator : AbstractValidator<PropertyAttributeDTO>
    {
        public PropertyAttributeDTOValidator()
        {
            RuleFor(x => x.AttributeId)
                .NotEmpty()
                .WithMessage("رقم الميزة مطلوب.");

            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage("قيمة الميزة مطلوبة.")
                .MaximumLength(1000)
                .WithMessage("قيمة الميزة يجب ألا تتجاوز 1000 محرف.");
        }
    }
}
