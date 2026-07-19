using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        private readonly IAuthService _authService;
        public LoginUserHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginUserAsync(request);
            return result;
        }
    }
}
