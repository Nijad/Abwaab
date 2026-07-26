using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class InitiateEmailChangeHandler : IRequestHandler<InitiateEmailChangeRequest, InitiateEmailChangeResponse>
    {
        private readonly IAuthService _authService;

        public InitiateEmailChangeHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<InitiateEmailChangeResponse> Handle(InitiateEmailChangeRequest request, CancellationToken cancellationToken)
        {
            InitiateEmailChangeResponse result = await _authService.InitiatieEmailChange(request);

            return result;
        }
    }
}
