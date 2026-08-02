using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Users.Auth.SendCode
{
    public class SendCodeDTO : IRequest<SendCodeResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public IdentifierEnum IdentifierType { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
