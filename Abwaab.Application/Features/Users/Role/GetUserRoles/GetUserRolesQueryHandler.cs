using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
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
        private readonly string errorTitle = ErrorTitle.GetUserRoles;

        public GetUserRolesQueryHandler(
            UserManager<ApplicationUser> userManager, 
            IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<GetUserRolesResponse> Handle(GetUserRolesDTO request, CancellationToken cancellationToken)
        {
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType, errorTitle);
            if (user == null)
                throw new UserNotFoundException(request.Identifier, errorTitle);

            var roles = await _userManager.GetRolesAsync(user);
            return new GetUserRolesResponse { Success = true, Roles = roles.ToList() };
        }
    }
}
