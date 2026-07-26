using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class CancelEmailChangeHandler : IRequestHandler<CancelEmailChangeRequest, CancelEmailChangeResponse>
    {
        IAuthService _authService;
        public CancelEmailChangeHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<CancelEmailChangeResponse> Handle(CancelEmailChangeRequest request, CancellationToken cancellationToken)
        {
            CancelEmailChangeResponse response = await _authService.CancelEmailChange(request);
            return response;
        }
    }
}
