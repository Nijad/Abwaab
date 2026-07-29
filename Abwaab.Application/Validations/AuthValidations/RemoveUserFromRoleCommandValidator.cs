using Abwaab.Application.DTOs.Roles.RemoveUserFormRole;
using FluentValidation;

namespace Abwaab.Application.Validations.AuthValidations
{
    public class RemoveUserFromRoleCommandValidator : AbstractValidator<RemoveUserFromRoleDTO>
    {
        public RemoveUserFromRoleCommandValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("User identifier is required.");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50);
        }
    }
}
