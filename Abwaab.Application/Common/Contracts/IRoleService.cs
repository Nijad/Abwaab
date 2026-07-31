using Abwaab.Application.Features.Users.Role.AddUserToRole;
using Abwaab.Application.Features.Users.Role.GetUserRoles;
using Abwaab.Application.Features.Users.Role.RemoveUserFromRole;

namespace Abwaab.Application.Common.Contracts
{
    public interface IRoleService
    {
        Task<AddUserToRoleResponse> AddUserToRoleCommandAsync(AddUserToRoleDTO request);
        Task<RemoveUserFromRoleResponse> RemoveUserFromRoleCommandAsync(RemoveUserFromRoleDTO request);
        Task<GetUserRolesResponse> GetUserRolesQueryAsync(GetUserRolesDTO request);
        Task<List<string>> GetAllRolesQueryAsync();
    }
}
