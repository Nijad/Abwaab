using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Role;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Role.RemoveUserFromRole
{
    public class RemoveUserFromRoleCommandHandler : IRequestHandler<RemoveUserFromRoleDTO, RemoveUserFromRoleResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _userService;
        private readonly ILogger<RemoveUserFromRoleCommandHandler> _logger;
        private readonly string errorTitle = ErrorTitle.RemoveUserFromRole;

        public RemoveUserFromRoleCommandHandler(
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, 
            IUserService userService, 
            ILogger<RemoveUserFromRoleCommandHandler> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _logger = logger;
        }
        public async Task<RemoveUserFromRoleResponse> Handle(RemoveUserFromRoleDTO request, CancellationToken cancellationToken)
        {
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType, errorTitle);
            if (user == null)
                throw new UserNotFoundException(request.Identifier, errorTitle);

            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
                throw new NotFoundException("Role", nameof(request.RoleName), request.RoleName, errorTitle);

            var isInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
            if (!isInRole)
                throw new UserNotInRoleException(user.UserName ,request.RoleName, errorTitle);

            var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                
                _logger.LogError($"Failed to remove user {user.Id} from role {request.RoleName}: {errors}", user.Id, request.RoleName, errors);

                throw new FailedToRemoveUserFromRoleException(errorTitle);
            }

            return new RemoveUserFromRoleResponse { Success = true, Message = $"تم استبعاد المستخدم من الدور '{request.RoleName}' بنجاح." };
        }
    }
}
