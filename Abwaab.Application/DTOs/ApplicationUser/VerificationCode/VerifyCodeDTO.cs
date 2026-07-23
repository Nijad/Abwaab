using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.VerificationCode
{
    public class VerifyCodeDTO : IRequest<VerifyCodeResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public IdentifierEnum IdentifierType { get; set; }
        public string Code { get; set; } = string.Empty;
    }

}
