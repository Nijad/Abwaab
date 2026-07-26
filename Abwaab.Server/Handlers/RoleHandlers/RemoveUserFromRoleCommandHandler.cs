using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.Roles.RemoveUserFormRole;
using MediatR;

namespace Abwaab.Server.Handlers.RoleHandlers
{
    public class RemoveUserFromRoleCommandHandler : IRequestHandler<RemoveUserFromRoleDTO, RemoveUserFromRoleResponse>
    {
        private readonly IAuthService _authService;
        public RemoveUserFromRoleCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<RemoveUserFromRoleResponse> Handle(RemoveUserFromRoleDTO request, CancellationToken cancellationToken)
        {
            RemoveUserFromRoleResponse response = await _authService.RemoveUserFromRoleCommandAsync(request);
            return response;
        }
    }
}
