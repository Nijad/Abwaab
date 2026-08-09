using FluentValidation;

namespace Abwaab.Application.Features.Plans.CreatePlan
{
    public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
    {
        public CreatePlanCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");
            
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");
            
            RuleFor(x => x.DurationInDays)
                .GreaterThan(0)
                .WithMessage("DurationInDays must be greater than 0.");
            
            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.ExpieryDate)
                .WithMessage("StartDate must be less than or equal to ExpieryDate.");
            
            RuleFor(x => x.TempDurationInDays)
                .GreaterThanOrEqualTo(0)
                .WithMessage("TempDurationInDays must be greater than or equal to 0.");
            
            RuleFor(x => x.MaxPropertiesCountAtSameTime)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MaxPropertiesCountAtSameTime must be greater than or equal to 0.");
            
            RuleFor(x => x.MaxStardPropertiesCountAtSameTime)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MaxStardPropertiesCountAtSameTime must be greater than or equal to 0.");
            
            RuleFor(x => x.MaxImagesCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MaxImagesCount must be greater than or equal to 0.");
            
            RuleFor(x => x.MaxVideosCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MaxVideosCount must be greater than or equal to 0.");
        }
    }
}
