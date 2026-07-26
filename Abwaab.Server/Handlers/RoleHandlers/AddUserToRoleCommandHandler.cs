using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.Roles.AddRoleToUser;
using MediatR;

namespace Abwaab.Server.Handlers.RoleHandlers
{
    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleDTO, AddUserToRoleResponse>
    {
        IAuthService _authService;
        public AddUserToRoleCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AddUserToRoleResponse> Handle(AddUserToRoleDTO request, CancellationToken cancellationToken)
        {
            AddUserToRoleResponse response = await _authService.AddUserToRoleCommandAsync(request);
            return response;
        }
    }
}
