namespace Abwaab.Domain.Entities.UserEntities
{
    public class UserPlanStatus : BaseEntity
    {
        public string StateName { get; set; } = string.Empty;
        public List<UserPlan>? UserPlans { get; set; }
    }
}
