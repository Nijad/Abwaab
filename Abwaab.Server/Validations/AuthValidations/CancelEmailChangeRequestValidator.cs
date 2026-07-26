using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using FluentValidation;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class CancelEmailChangeRequestValidator : AbstractValidator<CancelEmailChangeRequest>
    {
        // No rules needed – it's an empty request
    }
}
