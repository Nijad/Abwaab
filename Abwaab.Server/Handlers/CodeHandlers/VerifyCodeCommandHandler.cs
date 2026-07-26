using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using MediatR;

namespace Abwaab.Server.Handlers.CodeHandlers
{
    public class VerifyCodeCommandHandler : IRequestHandler<VerifyCodeDTO, VerifyCodeResponse>
    {
        private readonly IAuthService _authService;
        public VerifyCodeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<VerifyCodeResponse> Handle(VerifyCodeDTO request, CancellationToken cancellationToken)
        {
            VerifyCodeResponse result = await _authService.VerifyUserCommandAsync(request);
            return result;
        }
    }
}
