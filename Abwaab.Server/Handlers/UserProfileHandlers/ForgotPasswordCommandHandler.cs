using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.ForgotPassword;
using MediatR;

namespace Abwaab.Server.Handlers.UserProfileHandlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordDTO, ForgotPasswordResponse>
    {
        private readonly IAuthService _authService;
        public ForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.ForgotPasswordCommandAsyn(request);
            return result;
        }
    }
}
