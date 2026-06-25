namespace Abwaab.Domain.Entities.UserEntities
{
    public class OTP : BaseEntity
    {
        public string Code { get; set; } = null!;
        public DateTime ExpiredAt { get; set; } = DateTime.Now.AddMinutes(5);
        public bool IsUsed { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
