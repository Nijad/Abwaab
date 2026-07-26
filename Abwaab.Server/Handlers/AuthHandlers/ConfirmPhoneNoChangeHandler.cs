using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class ConfirmPhoneNoChangeHandler : IRequestHandler<ConfirmPhoneNoChangeRequest, ConfirmPhoneNoChangeResponse>
    {
        private readonly IAuthService _authService;
        public ConfirmPhoneNoChangeHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ConfirmPhoneNoChangeResponse> Handle(ConfirmPhoneNoChangeRequest request, CancellationToken cancellationToken)
        {
            ConfirmPhoneNoChangeResponse response = await _authService.ConfirmPhoneNoChange(request);
            
            return response;
        }
    }
}
