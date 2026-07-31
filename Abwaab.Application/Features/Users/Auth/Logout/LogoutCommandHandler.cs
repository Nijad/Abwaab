using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Auth.Logout
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
