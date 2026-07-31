using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Auth.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserDTO, LoginUserResponse>
    {
        private readonly IAuthService _authService;
        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<LoginUserResponse> Handle(LoginUserDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginUserCommandAsync(request);
            return result;
        }
    }
}
