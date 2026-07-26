using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.UserProfileHandlers
{
    public class ConfirmPhoneNoChangeCommandHandler : IRequestHandler<ConfirmPhoneNoChangeCommand, ConfirmPhoneNoChangeResponse>
    {
        private readonly IAuthService _authService;
        public ConfirmPhoneNoChangeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ConfirmPhoneNoChangeResponse> Handle(ConfirmPhoneNoChangeCommand request, CancellationToken cancellationToken)
        {
            ConfirmPhoneNoChangeResponse response = await _authService.ConfirmPhoneNoChangeCommandAsync(request);
            
            return response;
        }
    }
}
