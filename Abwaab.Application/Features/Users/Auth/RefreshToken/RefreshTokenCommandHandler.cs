using Abwaab.Application.Common.Interfaces;
using MediatR;

namespace Abwaab.Application.Features.Users.Auth.RefreshToken
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