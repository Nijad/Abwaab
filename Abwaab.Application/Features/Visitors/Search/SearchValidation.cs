using FluentValidation;

namespace Abwaab.Application.Features.Visitors.Search;

public class SearchValidation : AbstractValidator<SearchQuery>
{
    public SearchValidation()
    {
        RuleFor(x => x.MinPrice)
            .GreaterThan(0)
            .When(x => x.MinPrice.HasValue)
            .WithMessage("السعر الأدنى يجب أن يكون أكبر من الصفر.");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(x => x.MinPrice)
            .When(x => x.MaxPrice.HasValue && x.MinPrice.HasValue)
            .WithMessage("السعر الأقصى يجب أن يكون أكبر من أو يساوي السعر الأدنى.");

        RuleFor(x => x.MinArea)
            .GreaterThan(0)
            .When(x => x.MinArea.HasValue)
            .WithMessage("المساحة الدنيا يجب أن تكون أكبر من الصفر.");

        RuleFor(x => x.MaxArea)
            .GreaterThanOrEqualTo(x => x.MinArea)
            .When(x => x.MaxArea.HasValue && x.MinArea.HasValue)
            .WithMessage("المساحة القصوى يجب أن تكون أكبر من أو يساوي المساحة الدنيا.");
    }
}