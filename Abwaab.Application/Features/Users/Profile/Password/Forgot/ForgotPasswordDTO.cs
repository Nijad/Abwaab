using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Password.Forgot
{
    public class ForgotPasswordDTO : IRequest<ForgotPasswordResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public IdentifiersEnum IdentifierType { get; set; }
    }
}
