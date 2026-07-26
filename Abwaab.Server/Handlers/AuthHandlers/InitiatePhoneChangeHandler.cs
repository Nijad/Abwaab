using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class InitiatePhoneChangeHandler : IRequestHandler<InitiatePhoneNoChangeRequest, InitiatePhoneNoChangeResponse>
    {
        private readonly IAuthService _authService;
        public InitiatePhoneChangeHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<InitiatePhoneNoChangeResponse> Handle(InitiatePhoneNoChangeRequest request, CancellationToken cancellationToken)
        {
            InitiatePhoneNoChangeResponse response = await _authService.InitiatePhoneNoChange(request);
            return response;
        }
    }
}
