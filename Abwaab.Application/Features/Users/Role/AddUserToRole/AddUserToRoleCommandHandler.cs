using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
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
        private readonly string errorTitle = ErrorTitle.AssignRoleToUser;
        
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
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType, errorTitle);
            if (user == null)
                throw new UserNotFoundException(request.Identifier, errorTitle);

            // 2. Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
                throw new NotFoundException("Role", nameof(request.RoleName), request.RoleName, errorTitle);

            // 3. Check if user already has the role
            var isInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
            if (isInRole)
                throw new UserAlreadyInRoleException(user.UserName, request.RoleName, errorTitle);

            // 4. Add the role
            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                
                _logger.LogError($"Failed to add user {user.Id} to '{request.RoleName}' role : {errors}");

                throw new FailedToAddUserToRoleException(errorTitle);
            }

            return new AddUserToRoleResponse { Success = true, Message = $"تم إضافة المستخدم إلى الدور '{request.RoleName}' بنجاح." };
        }
    }
}
