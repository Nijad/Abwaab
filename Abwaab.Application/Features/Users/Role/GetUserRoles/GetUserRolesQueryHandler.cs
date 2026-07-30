using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Role.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesDTO, GetUserRolesResponse>
    {
        private readonly IRoleService _roleService;
        public GetUserRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetUserRolesResponse> Handle(GetUserRolesDTO request, CancellationToken cancellationToken)
        {
            GetUserRolesResponse response = await _roleService.GetUserRolesQueryAsync(request);
            return response;
        }
    }
}
