using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class CancelPhoneChangeHandler : IRequestHandler<CancelPhoneChangeRequest, CancelPhoneChangeResponse>
    {
        IAuthService _authService;
        public CancelPhoneChangeHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<CancelPhoneChangeResponse> Handle(CancelPhoneChangeRequest request, CancellationToken cancellationToken)
        {
            CancelPhoneChangeResponse response = await _authService.CancelPhoneChange(request);
            return response;
        }
    }
}
