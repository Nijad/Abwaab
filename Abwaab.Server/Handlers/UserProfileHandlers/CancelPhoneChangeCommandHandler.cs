using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.UserProfileHandlers
{
    public class CancelPhoneChangeCommandHandler : IRequestHandler<CancelPhoneChangeCommand, CancelPhoneChangeResponse>
    {
        IAuthService _authService;
        public CancelPhoneChangeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<CancelPhoneChangeResponse> Handle(CancelPhoneChangeCommand request, CancellationToken cancellationToken)
        {
            CancelPhoneChangeResponse response = await _authService.CancelPhoneChangeCommandAsync(request);
            return response;
        }
    }
}
