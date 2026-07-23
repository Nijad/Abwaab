using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.LoginUser
{
    public class LoginUserRequest : IRequest<LoginUserResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
