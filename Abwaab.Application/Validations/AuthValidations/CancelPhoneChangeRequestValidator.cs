using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using FluentValidation;

namespace Abwaab.Application.Validations.AuthValidations
{
    public class CancelPhoneChangeRequestValidator : AbstractValidator<CancelPhoneChangeCommand>
    {
        // No rules needed
    }
}
