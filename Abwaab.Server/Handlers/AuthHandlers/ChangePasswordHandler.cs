using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.ChangePassword;
using Abwaab.Infrastructure.Services.UserServices;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordDTO, ChangePasswordResponse>
    {
        private readonly IAuthService _authService;
        public ChangePasswordHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ChangePasswordResponse> Handle(ChangePasswordDTO request, CancellationToken cancellationToken)
        {
            // Implement the logic to change the password here
            // For now, we will return a dummy response
            ChangePasswordResponse result = await _authService.ChangePassword(request);
            return await Task.FromResult(result);
        }
    }
}
