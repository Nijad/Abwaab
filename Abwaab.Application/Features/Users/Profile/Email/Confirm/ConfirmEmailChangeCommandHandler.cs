using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Email.Confirm
{
    public class ConfirmEmailChangeCommandHandler : IRequestHandler<ConfirmEmailChangeCommand, ConfirmEmailChangeResponse>
    {
        private readonly IProfileService _profileService;

        public ConfirmEmailChangeCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<ConfirmEmailChangeResponse> Handle(ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
        {
            ConfirmEmailChangeResponse result = await _profileService.ConfirmEmailChangeCommandAsync(request);

            return result;
        }
    }
}
