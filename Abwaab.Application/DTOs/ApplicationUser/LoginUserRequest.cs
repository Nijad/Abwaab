using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class LoginUserRequest : IRequest<LoginUserResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
