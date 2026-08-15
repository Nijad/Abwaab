using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Role.AddUserToRole
{
    public class AddUserToRoleCommandValidation : AbstractValidator<AddUserToRoleDTO>
    {
        public AddUserToRoleCommandValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("المعرف مطلوب")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("المعرف يجب أن يكون بريد الكتروني أو رقم موبايل (+9639XXXXXXXX)");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("الدور مطلوب.");
        }
    }
}
