using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class ResendCodeRequest : IRequest<ResendCodeResponse>
    {
        public string Identifier { get; set; } = string.Empty;
    }
}
