using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RefreshTokenCommandHandler(
            IJwtService jwtService,
            UserManager<ApplicationUser> userManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            Guid userId = await _jwtService.GetUserIdByTokenAsync(request);

            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new NotFoundException("User", nameof(userId), userId.ToString());

            IList<string> roles = await _userManager.GetRolesAsync(user);

            RefreshTokenResponse reuslt = await _jwtService.RefreshTokenAsync(user, roles, request.RefreshToken);

            return await Task.FromResult(reuslt);
        }
    }
}