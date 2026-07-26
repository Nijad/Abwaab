using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.VerificationCode
{
    public class ResendCodeDTO : IRequest<ResendCodeResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public IdentifierEnum IdentifierType { get; set; }
    }
}
