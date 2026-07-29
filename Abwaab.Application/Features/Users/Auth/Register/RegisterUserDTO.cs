using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Users.Auth.Register
{
    public class RegisterUserDTO : IRequest<RegisterUserResponse>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public IdentifierEnum IdentifierType { get; set; }
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
