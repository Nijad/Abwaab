
using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class RegisterUserByEmailHandler : IRequestHandler<RegisterUserByEmailRequest, RegisterUserResponse>
    {
        private readonly IAuthService _authService;

        public RegisterUserByEmailHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<RegisterUserResponse> Handle(RegisterUserByEmailRequest request, CancellationToken cancellationToken)
        {
            RegisterUserResponse result = await _authService.RegisterUserByEmailAsync(request);
            return result;
        }
    }
}
