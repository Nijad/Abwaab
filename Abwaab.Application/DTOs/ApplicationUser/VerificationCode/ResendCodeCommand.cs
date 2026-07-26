using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.VerificationCode
{
    public class ResendCodeCommand : IRequest<ResendCodeResponse>
    {
        public string Identifier { get; set; } = string.Empty;
    }
}
