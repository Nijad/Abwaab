using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Password.Reset
{
    public class ResetPasswordDTO : IRequest<ResetPasswordResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public IdentifiersEnum IdentifierType { get; set; }
        public string Code { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
