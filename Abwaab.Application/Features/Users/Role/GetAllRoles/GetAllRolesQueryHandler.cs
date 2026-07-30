using Abwaab.Application.Common.Contracts;
using MediatR;

namespace Abwaab.Application.Features.Users.Role.GetAllRoles
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<string>>
    {
        private readonly IRoleService _roleService;
        public GetAllRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<List<string>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            List<string> response = await _roleService.GetAllRolesQueryAsync();
            return response;
        }
    }
}
