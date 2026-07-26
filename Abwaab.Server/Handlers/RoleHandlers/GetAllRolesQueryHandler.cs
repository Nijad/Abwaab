using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.Roles.GetAllRoles;
using MediatR;

namespace Abwaab.Server.Handlers.RoleHandlers
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<string>>
    {
        private readonly IAuthService _authService;
        public GetAllRolesQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<List<string>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            List<string> response = await _authService.GetAllRolesQueryAsync();
            return response;
        }
    }
}
