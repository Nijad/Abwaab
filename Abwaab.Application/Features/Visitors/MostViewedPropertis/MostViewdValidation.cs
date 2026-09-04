using FluentValidation;

namespace Abwaab.Application.Features.Visitors.MostViewedPropertis;

public class MostViewdValidation:AbstractValidator<MostViewedQuery>
{
    public MostViewdValidation()
    {
        RuleFor(x => x.PageNo)
            .GreaterThan(0).WithMessage("رقم الصفحة المطلوب يجب أن يكون أكبر من الصفر.");
    }
}