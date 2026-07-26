using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class ConfirmEmailChangeHandler : IRequestHandler<ConfirmEmailChangeRequest, ConfirmEmailChangeResponse>
    {
        private readonly IAuthService _authService;

        public ConfirmEmailChangeHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ConfirmEmailChangeResponse> Handle(ConfirmEmailChangeRequest request, CancellationToken cancellationToken)
        {
            ConfirmEmailChangeResponse result = await _authService.ConfirmEmailChange(request);

            return result;
        }
    }
}
