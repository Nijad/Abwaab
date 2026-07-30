using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Email.Confirm
{
    public class ConfirmEmailChangeCommand : IRequest<ConfirmEmailChangeResponse>
    {
        public string NewEmail { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
