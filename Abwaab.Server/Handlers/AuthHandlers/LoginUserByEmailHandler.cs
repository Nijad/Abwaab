using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class LoginUserByEmailHandler : IRequestHandler<LoginUserByEmailRequest, LoginUserResponse>
    {
        private readonly IAuthService _authService;
        public LoginUserByEmailHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<LoginUserResponse> Handle(LoginUserByEmailRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginUserByEmailAsync(request);
            return result;
        }
    }
}
