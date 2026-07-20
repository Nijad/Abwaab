using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class VerifyCodeRequest : IRequest<VerifyCodeResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        //public string? Email { get; set; }
        //public string? PhoneNumber { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
