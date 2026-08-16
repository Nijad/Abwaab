using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Abwaab.Application.Features.Users.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string errorTitle = ErrorTitle.RefreshToken;

        public RefreshTokenCommandHandler(
            IJwtService jwtService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            Guid userId = await _jwtService.GetUserIdByTokenAsync(request, errorTitle);

            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new UserNotFoundException(userId.ToString(), errorTitle);

            IList<string> roles = await _userManager.GetRolesAsync(user);

            var httpContext = _contextAccessor.HttpContext;
            var oldLoginIdentifier = httpContext.User.FindFirstValue("LoginIdentifier") ?? user.Email ?? user.PhoneNumber;
            RefreshTokenResponse reuslt = await _jwtService.RefreshTokenAsync(user, roles, request.RefreshToken, oldLoginIdentifier);

            return await Task.FromResult(reuslt);
        }
    }
}