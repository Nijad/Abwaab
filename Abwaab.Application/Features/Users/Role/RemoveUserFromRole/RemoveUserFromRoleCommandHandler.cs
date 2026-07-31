using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Role.RemoveUserFromRole
{
    public class RemoveUserFromRoleCommandHandler : IRequestHandler<RemoveUserFromRoleDTO, RemoveUserFromRoleResponse>
    {
        private readonly IRoleService _roleService;
        public RemoveUserFromRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<RemoveUserFromRoleResponse> Handle(RemoveUserFromRoleDTO request, CancellationToken cancellationToken)
        {
            RemoveUserFromRoleResponse response = await _roleService.RemoveUserFromRoleCommandAsync(request);
            return response;
        }
    }
}
