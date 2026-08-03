using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Role.AddUserToRole
{
    public class AddUserToRoleCommandValidation : AbstractValidator<AddUserToRoleDTO>
    {
        public AddUserToRoleCommandValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("User identifier is required.")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("Identifier must be either valide email or valid phone number(+9639XXXXXXXX)");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50);
        }
    }
}
