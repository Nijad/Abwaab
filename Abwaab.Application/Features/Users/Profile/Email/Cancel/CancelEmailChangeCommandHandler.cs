using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Email.Cancel
{
    public class CancelEmailChangeCommandHandler : IRequestHandler<CancelEmailChangeCommand, CancelEmailChangeResponse>
    {
        private readonly IProfileService _profileService;
        public CancelEmailChangeCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }
        public async Task<CancelEmailChangeResponse> Handle(CancelEmailChangeCommand request, CancellationToken cancellationToken)
        {
            CancelEmailChangeResponse response = await _profileService.CancelEmailChangeCommandAsync(request);
            return response;
        }
    }
}
