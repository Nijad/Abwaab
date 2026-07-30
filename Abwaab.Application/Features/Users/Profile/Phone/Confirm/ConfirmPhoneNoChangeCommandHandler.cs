using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Phone.Confirm
{
    public class ConfirmPhoneNoChangeCommandHandler : IRequestHandler<ConfirmPhoneNoChangeCommand, ConfirmPhoneNoChangeResponse>
    {
        private readonly IProfileService _profileService;
        public ConfirmPhoneNoChangeCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }
        public async Task<ConfirmPhoneNoChangeResponse> Handle(ConfirmPhoneNoChangeCommand request, CancellationToken cancellationToken)
        {
            ConfirmPhoneNoChangeResponse response = await _profileService.ConfirmPhoneNoChangeCommandAsync(request);
            
            return response;
        }
    }
}
