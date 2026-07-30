using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Email.InitiateChange
{
    public class InitiateEmailChangeCommandHandler : IRequestHandler<InitiateEmailChangeCommand, InitiateEmailChangeResponse>
    {
        private readonly IProfileService _profileService;

        public InitiateEmailChangeCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<InitiateEmailChangeResponse> Handle(InitiateEmailChangeCommand request, CancellationToken cancellationToken)
        {
            InitiateEmailChangeResponse result = await _profileService.InitiatieEmailChangeCommandAsync(request);

            return result;
        }
    }
}
