using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Role;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Role.AddUserToRole
{
    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleDTO, AddUserToRoleResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AddUserToRoleCommandHandler> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _userService;
        
        public AddUserToRoleCommandHandler(
            UserManager<ApplicationUser> userManager, 
            ILogger<AddUserToRoleCommandHandler> logger,
            RoleManager<ApplicationRole> roleManager, 
            IUserService userService)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _userService = userService;
        }

        public async Task<AddUserToRoleResponse> Handle(AddUserToRoleDTO request, CancellationToken cancellationToken)
        {
            // 1. Find the user 
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                throw new NotFoundException("User", request.IdentifierType.ToString().Replace('_', ' '), request.Identifier);

            // 2. Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
                throw new NotFoundException("Role", nameof(request.RoleName), request.RoleName);

            // 3. Check if user already has the role
            var isInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
            if (isInRole)
                throw new UserAlreadyInRoleException(request.RoleName);

            // 4. Add the role
            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                
                _logger.LogError($"Failed to add user {user.Id} to '{request.RoleName}' role : {errors}");

                throw new FailedToAddUserToRoleException();
            }

            return new AddUserToRoleResponse { Success = true, Message = $"User added to role '{request.RoleName}' successfully." };
        }
    }
}
