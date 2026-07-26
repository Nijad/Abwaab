using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.ChangePassword;
using MediatR;

namespace Abwaab.Server.Handlers.UserProfileHandlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordDTO, ChangePasswordResponse>
    {
        private readonly IAuthService _authService;
        public ChangePasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ChangePasswordResponse> Handle(ChangePasswordDTO request, CancellationToken cancellationToken)
        {
            // Implement the logic to change the password here
            // For now, we will return a dummy response
            ChangePasswordResponse result = await _authService.ChangePasswordCommandAsync(request);
            return await Task.FromResult(result);
        }
    }
}
