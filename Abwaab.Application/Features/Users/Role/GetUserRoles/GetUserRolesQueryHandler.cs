using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Role.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesDTO, GetUserRolesResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        public GetUserRolesQueryHandler(
            UserManager<ApplicationUser> userManager, 
            IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<GetUserRolesResponse> Handle(GetUserRolesDTO request, CancellationToken cancellationToken)
        {
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                throw new NotFoundException("User", request.IdentifierType.ToString().Replace('_', ' '), request.Identifier);

            var roles = await _userManager.GetRolesAsync(user);
            return new GetUserRolesResponse { Success = true, Roles = roles.ToList() };
        }
    }
}
