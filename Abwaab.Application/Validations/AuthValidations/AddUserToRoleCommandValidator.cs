using Abwaab.Application.DTOs.Roles.AddRoleToUser;
using FluentValidation;

namespace Abwaab.Application.Validations.AuthValidations
{
    public class AddUserToRoleCommandValidator : AbstractValidator<AddUserToRoleDTO>
    {
        public AddUserToRoleCommandValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("User identifier is required.");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50);
        }
    }
}
