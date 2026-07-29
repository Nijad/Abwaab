using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Auth.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserDTO, RegisterUserResponse>
    {
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<RegisterUserResponse> Handle(RegisterUserDTO request, CancellationToken cancellationToken)
        {
            RegisterUserResponse result = await _authService.RegisterUserCommandAsync(request);
            return result;
        }
    }
}
