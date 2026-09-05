using FluentValidation;

namespace Abwaab.Application.Features.Visitors.RecentlyAddedProperties;

public class RecentlyAddedValidation : AbstractValidator<RecentlyAddedQuery>
{
    public RecentlyAddedValidation()
    {
        RuleFor(x => x.PageNo)
            .GreaterThan(0).WithMessage("رقم الصفحة المطلوب يجب أن يكون أكبر من الصفر.");
    }
}
