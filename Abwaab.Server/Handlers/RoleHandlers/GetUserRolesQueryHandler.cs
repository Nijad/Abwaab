using Abwaab.Application.Common.Contracts;
using Abwaab.Application.DTOs.Roles.GetUserRoles;
using MediatR;

namespace Abwaab.Server.Handlers.RoleHandlers
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesDTO, GetUserRolesResponse>
    {
        private readonly IAuthService _authService;
        public GetUserRolesQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<GetUserRolesResponse> Handle(GetUserRolesDTO request, CancellationToken cancellationToken)
        {
            GetUserRolesResponse response = await _authService.GetUserRolesQueryAsync(request);
            return response;
        }
    }
}
