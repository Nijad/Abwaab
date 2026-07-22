using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordDTO, ForgotPasswordResponse>
    {
        private readonly IAuthService _authService;
        public ForgotPasswordHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.ForgotPasswordAsyn(request);
            return result;
        }
    }
}
