using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class LoginUserDTO : IRequest<LoginUserResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public IdentifierEnum IdentifierType { get; set; }
        public string Password { get; set; } = string.Empty;
    }

}
