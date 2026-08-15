using FluentValidation;

namespace Abwaab.Application.Features.Users.Auth.Logout
{
    public class LogoutValidation : AbstractValidator<LogoutCommand>
    {
        public LogoutValidation()
        {
        }
    }
}
