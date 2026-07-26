using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.UserProfileHandlers
{
    public class CancelEmailChangeCommandHandler : IRequestHandler<CancelEmailChangeCommand, CancelEmailChangeResponse>
    {
        IAuthService _authService;
        public CancelEmailChangeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<CancelEmailChangeResponse> Handle(CancelEmailChangeCommand request, CancellationToken cancellationToken)
        {
            CancelEmailChangeResponse response = await _authService.CancelEmailChangeCommandAsync(request);
            return response;
        }
    }
}
