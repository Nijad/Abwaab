using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class RegisterRequest: IRequest<RegisterUserResponse>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        //public string Email { get; set; } = string.Empty;
        //public string PhoneNo { get; set; } = string.Empty;
        //public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
