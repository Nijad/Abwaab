using FluentValidation;

namespace Abwaab.Application.Features.Plans.CreatePlan
{
    public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
    {
        public CreatePlanCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("اسم الخطة مطلوب");
            
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("السعر يجب أن يكون أكبر من الصفر");
            
            RuleFor(x => x.DurationInDays)
                .GreaterThan(0)
                .WithMessage("المدة بالأيام يجب أن تكون أكبر من الصفر");
            
            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.ExpieryDate)
                .WithMessage("تاريخ البداية يجب أن يكون أصغر أو يساوي تاريخ الانتهاء");
            
            RuleFor(x => x.TempDurationInDays)
                .GreaterThanOrEqualTo(0)
                .WithMessage("عدد أيام السماح يجب أن يكون أكبر أو يساوي الصفر");
            
            RuleFor(x => x.MaxPropertiesCountAtSameTime)
                .GreaterThanOrEqualTo(0)
                .WithMessage("العدد الأعظمي للعقارات يجب أن يكون أكبر أو يساوي الصفر");
            
            RuleFor(x => x.MaxStardPropertiesCountAtSameTime)
                .GreaterThanOrEqualTo(0)
                .WithMessage("العدد الأعظمي للعقارات المميزة يجب أن يكون أكبر أو يساوي الصفر");
            
            RuleFor(x => x.MaxImagesCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("العدد الأعظمي للصور يجب أن يكون أكبر أو يساوي الصفر");
            
            RuleFor(x => x.MaxVideosCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("العدد الأعظمي لملفات الفيديو يجب أن يكون أكبر أو يساوي الصفر");
        }
    }
}
