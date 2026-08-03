using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Password.VerifyResetCode
{
    public class VerifyResetCodeDTO : IRequest<VerifyResetCodeResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public IdentifierEnum IdentifierType { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
