using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.LogoutUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class LogoutHandler : IRequestHandler<LogoutRequest, LogoutResponse>
    {
        private readonly IAuthService _authService;
        public LogoutHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            LogoutResponse response = await _authService.Logout(request);

            return response;
        }
    }
}
