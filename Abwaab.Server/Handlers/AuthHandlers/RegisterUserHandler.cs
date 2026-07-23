
using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.RegisterUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserDTO, RegisterUserResponse>
    {
        private readonly IAuthService _authService;

        public RegisterUserHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<RegisterUserResponse> Handle(RegisterUserDTO request, CancellationToken cancellationToken)
        {
            RegisterUserResponse result = await _authService.RegisterUserAsync(request);
            return result;
        }
    }
}
