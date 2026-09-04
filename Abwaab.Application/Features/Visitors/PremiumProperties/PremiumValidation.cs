using FluentValidation;

namespace Abwaab.Application.Features.Visitors.PremiumProperties;

public class PremiumValidation : AbstractValidator<PremiumQuery>
{
    public PremiumValidation()
    {
        RuleFor(x => x.PageNo)
            .GreaterThan(0).WithMessage("رقم الصفحة المطلوب يجب أن يكون أكبر من الصفر.");
    }
}
