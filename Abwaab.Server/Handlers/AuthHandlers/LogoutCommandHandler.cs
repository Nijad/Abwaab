using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.LogoutUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutResponse>
    {
        private readonly IAuthService _authService;
        public LogoutCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LogoutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            LogoutResponse response = await _authService.LogoutCommandAsync(request);

            return response;
        }
    }
}
