
using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class VerifyCodeHandler : IRequestHandler<VerifyCodeDTO, VerifyCodeResponse>
    {
        private readonly IAuthService _authService;
        public VerifyCodeHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<VerifyCodeResponse> Handle(VerifyCodeDTO request, CancellationToken cancellationToken)
        {
            VerifyCodeResponse result = await _authService.VerifyUserAsync(request);
            return result;
        }
    }
}
