using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Phone.InitiateChange
{
    public class InitiatePhoneChangeCommandHandler : IRequestHandler<InitiatePhoneNoChangeCommand, InitiatePhoneNoChangeResponse>
    {
        private readonly IProfileService _profileService;
        public InitiatePhoneChangeCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<InitiatePhoneNoChangeResponse> Handle(InitiatePhoneNoChangeCommand request, CancellationToken cancellationToken)
        {
            InitiatePhoneNoChangeResponse response = await _profileService.InitiatePhoneNoChangeCommandAsync(request);
            return response;
        }
    }
}
