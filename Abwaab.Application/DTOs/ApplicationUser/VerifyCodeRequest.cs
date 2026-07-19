using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class VerifyCodeRequest : IRequest<VerifyCodeResponse>
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
