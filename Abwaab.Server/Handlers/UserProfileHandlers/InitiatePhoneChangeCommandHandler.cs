using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.UserProfileHandlers
{
    public class InitiatePhoneChangeCommandHandler : IRequestHandler<InitiatePhoneNoChangeCommand, InitiatePhoneNoChangeResponse>
    {
        private readonly IAuthService _authService;
        public InitiatePhoneChangeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<InitiatePhoneNoChangeResponse> Handle(InitiatePhoneNoChangeCommand request, CancellationToken cancellationToken)
        {
            InitiatePhoneNoChangeResponse response = await _authService.InitiatePhoneNoChangeCommandAsync(request);
            return response;
        }
    }
}
