using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Phone.InitiateChange
{
    public class InitiatePhoneNoChangeCommand : IRequest<InitiatePhoneNoChangeResponse>
    {
        public string NewPhoneNo { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
    }
}
