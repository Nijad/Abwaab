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
                .WithMessage(x=>$"رقم الميزة '{x.AttributeName}' مطلوب.");

            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage(x=>$"قيمة الميزة '{x.AttributeName}' مطلوبة.")
                .MaximumLength(1000)
                .WithMessage(x=>$"قيمة الميزة '{x.AttributeName}' يجب ألا تتجاوز 1000 محرف.");
        }
    }
}
