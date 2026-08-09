using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Email.InitiateChange
{
    public class InitiateEmailChangeCommand : IRequest<InitiateEmailChangeResponse>
    {
        public string NewEmail { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
    }
}
