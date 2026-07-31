using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Phone.Cancel
{
    public class CancelPhoneChangeCommandHandler : IRequestHandler<CancelPhoneChangeCommand, CancelPhoneChangeResponse>
    {
        IProfileService _profileService;
        public CancelPhoneChangeCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }
        public async Task<CancelPhoneChangeResponse> Handle(CancelPhoneChangeCommand request, CancellationToken cancellationToken)
        {
            CancelPhoneChangeResponse response = await _profileService.CancelPhoneChangeCommandAsync(request);
            return response;
        }
    }
}
