using FluentValidation;

namespace Abwaab.Application.Features.Users.Profile.UserPlans.Cancel
{
    public class CancelUserPlanVCommandalidation : AbstractValidator<CancelUserPlanCommand>
    {
        public CancelUserPlanVCommandalidation()
        {
            RuleFor(x => x.UserPlanId)
                .NotEmpty().WithMessage("Plan Id is required");
        }
    }
}
