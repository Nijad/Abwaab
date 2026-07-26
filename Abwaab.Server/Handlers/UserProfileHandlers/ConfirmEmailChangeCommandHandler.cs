using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.UserProfileHandlers
{
    public class ConfirmEmailChangeCommandHandler : IRequestHandler<ConfirmEmailChangeCommand, ConfirmEmailChangeResponse>
    {
        private readonly IAuthService _authService;

        public ConfirmEmailChangeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ConfirmEmailChangeResponse> Handle(ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
        {
            ConfirmEmailChangeResponse result = await _authService.ConfirmEmailChangeCommandAsync(request);

            return result;
        }
    }
}
