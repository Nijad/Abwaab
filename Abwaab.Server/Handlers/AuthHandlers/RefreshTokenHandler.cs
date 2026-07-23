using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        private readonly IJwtService _jwtService;
        

        public RefreshTokenHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            RefreshTokenResponse reuslt = await _jwtService.RefreshToken(request);
            return await Task.FromResult(reuslt);
        }
    }
}