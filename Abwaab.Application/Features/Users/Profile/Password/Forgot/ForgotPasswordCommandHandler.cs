using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Password.Forgot
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordDTO, ForgotPasswordResponse>
    {
        private readonly IProfileService _profileService;
        public ForgotPasswordCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }
        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordDTO request, CancellationToken cancellationToken)
        {
            var result = await _profileService.ForgotPasswordCommandAsyn(request);
            return result;
        }
    }
}
