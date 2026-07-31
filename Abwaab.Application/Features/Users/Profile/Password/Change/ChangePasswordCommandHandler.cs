using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Password.Change
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordDTO, ChangePasswordResponse>
    {
        private readonly IProfileService _profileService;
        public ChangePasswordCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<ChangePasswordResponse> Handle(ChangePasswordDTO request, CancellationToken cancellationToken)
        {
            // Implement the logic to change the password here
            // For now, we will return a dummy response
            ChangePasswordResponse result = await _profileService.ChangePasswordCommandAsync(request);
            return await Task.FromResult(result);
        }
    }
}
