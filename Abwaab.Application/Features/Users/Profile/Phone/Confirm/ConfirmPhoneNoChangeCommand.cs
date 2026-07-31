using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Phone.Confirm
{
    public class ConfirmPhoneNoChangeCommand : IRequest<ConfirmPhoneNoChangeResponse>
    {
        public string NewPhoneNo { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
