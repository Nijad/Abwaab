using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.UserProfileHandlers
{
    public class InitiateEmailChangeCommandHandler : IRequestHandler<InitiateEmailChangeCommand, InitiateEmailChangeResponse>
    {
        private readonly IAuthService _authService;

        public InitiateEmailChangeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<InitiateEmailChangeResponse> Handle(InitiateEmailChangeCommand request, CancellationToken cancellationToken)
        {
            InitiateEmailChangeResponse result = await _authService.InitiatieEmailChangeCommandAsync(request);

            return result;
        }
    }
}
