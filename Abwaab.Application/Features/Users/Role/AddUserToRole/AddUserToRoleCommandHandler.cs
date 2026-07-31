using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Role.AddUserToRole
{
    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleDTO, AddUserToRoleResponse>
    {
        IRoleService _roleService;
        public AddUserToRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<AddUserToRoleResponse> Handle(AddUserToRoleDTO request, CancellationToken cancellationToken)
        {
            AddUserToRoleResponse response = await _roleService.AddUserToRoleCommandAsync(request);
            return response;
        }
    }
}
