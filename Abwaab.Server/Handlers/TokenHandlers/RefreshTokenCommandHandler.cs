using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser.RefreshToken;
using MediatR;

namespace Abwaab.Server.Handlers.TokenHandlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IJwtService _jwtService;
        

        public RefreshTokenCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            RefreshTokenResponse reuslt = await _jwtService.RefreshToken(request);
            return await Task.FromResult(reuslt);
        }
    }
}