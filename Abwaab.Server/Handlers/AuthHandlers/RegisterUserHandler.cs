
using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterRequest, RegisterUserResponse>
    {
        private readonly IAuthService _authService;

        public RegisterUserHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<RegisterUserResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            RegisterUserResponse result = await _authService.RegisterUserAsync(request);
            return result;
        }
    }
}
