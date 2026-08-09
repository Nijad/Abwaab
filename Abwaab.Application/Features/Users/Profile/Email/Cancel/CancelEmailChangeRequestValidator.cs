using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.Email.Cancel
{
    public class CancelEmailChangeRequestValidator : AbstractValidator<CancelEmailChangeCommand>
    {
        // No rules needed – it's an empty request
    }
}
