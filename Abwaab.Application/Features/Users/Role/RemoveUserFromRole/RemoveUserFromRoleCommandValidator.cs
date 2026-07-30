using FluentValidation;

namespace Abwaab.Application.Features.Users.Role.RemoveUserFromRole
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
