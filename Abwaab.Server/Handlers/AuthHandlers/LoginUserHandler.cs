using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Options;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserDTO, LoginUserResponse>
    {
        private readonly IAuthService _authService;
        public LoginUserHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<LoginUserResponse> Handle(LoginUserDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginUserAsync(request);
            return result;
        }
    }
}
