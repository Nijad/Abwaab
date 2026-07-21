using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class VerifyCodeRequest : IRequest<VerifyCodeResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
