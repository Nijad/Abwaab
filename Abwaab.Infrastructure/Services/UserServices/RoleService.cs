using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Features.Users.Role.AddUserToRole;
using Abwaab.Application.Features.Users.Role.GetUserRoles;
using Abwaab.Application.Features.Users.Role.RemoveUserFromRole;
using Abwaab.Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoleService> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _userService;
        public RoleService(
            UserManager<ApplicationUser> userManager,
            ILogger<RoleService> logger,
            RoleManager<ApplicationRole> roleManager,
            IUserService userService)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _userService = userService;
        }

        public async Task<AddUserToRoleResponse> AddUserToRoleCommandAsync(AddUserToRoleDTO request)
        {
            // 1. Find the user 
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                return new AddUserToRoleResponse { Success = false, Message = "User not found." };

            // 2. Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
                return new AddUserToRoleResponse { Success = false, Message = $"Role '{request.RoleName}' does not exist." };

            // 3. Check if user already has the role
            var isInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
            if (isInRole)
                return new AddUserToRoleResponse { Success = false, Message = $"User already has the role '{request.RoleName}'." };

            // 4. Add the role
            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError($"Failed to add user {user.Id} to role {request.RoleName}: {errors}", user.Id, request.RoleName, errors);
                return new AddUserToRoleResponse { Success = false, Message = $"Failed: {errors}" };
            }

            _logger.LogInformation($"User {user.Id} added to role {request.RoleName}", user.Id, request.RoleName);
            return new AddUserToRoleResponse { Success = true, Message = $"User added to role '{request.RoleName}' successfully." };
        }

        public async Task<List<string>> GetAllRolesQueryAsync()
        {
            return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<GetUserRolesResponse> GetUserRolesQueryAsync(GetUserRolesDTO request)
        {
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                return new GetUserRolesResponse { Success = false, Message = "User not found." };

            var roles = await _userManager.GetRolesAsync(user);
            return new GetUserRolesResponse { Success = true, Roles = roles.ToList() };
        }

        public async Task<RemoveUserFromRoleResponse> RemoveUserFromRoleCommandAsync(RemoveUserFromRoleDTO request)
        {
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                return new RemoveUserFromRoleResponse { Success = false, Message = "User not found." };

            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
                return new RemoveUserFromRoleResponse { Success = false, Message = $"Role '{request.RoleName}' does not exist." };

            var isInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
            if (!isInRole)
                return new RemoveUserFromRoleResponse { Success = false, Message = $"User does not have the role '{request.RoleName}'." };

            var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError($"Failed to remove user {user.Id} from role {request.RoleName}: {errors}", user.Id, request.RoleName, errors);
                return new RemoveUserFromRoleResponse { Success = false, Message = $"Failed: {errors}" };
            }

            _logger.LogInformation($"User {user.Id} removed from role {request.RoleName}", user.Id, request.RoleName);
            return new RemoveUserFromRoleResponse { Success = true, Message = $"User removed from role '{request.RoleName}' successfully." };
        }
    }
}
