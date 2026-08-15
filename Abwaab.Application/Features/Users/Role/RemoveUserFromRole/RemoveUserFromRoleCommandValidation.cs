using Abwaab.Application.Common.Validations.Common;
using FluentValidation;

namespace Abwaab.Application.Features.Users.Role.RemoveUserFromRole
{
    public class RemoveUserFromRoleCommandValidation : AbstractValidator<RemoveUserFromRoleDTO>
    {
        public RemoveUserFromRoleCommandValidation()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("المعرف مطلوب")
                .Must(CommonValidation.IsEmailOrPhoneNo).WithMessage("المعرف يجب أن يكون بريد الكتروني أو رقم موبايل (+9639XXXXXXXX)");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("الدور مطلوب.");
        }
    }
}
