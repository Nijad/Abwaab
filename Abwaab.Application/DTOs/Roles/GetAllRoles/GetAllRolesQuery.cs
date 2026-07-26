using MediatR;

namespace Abwaab.Application.DTOs.Roles.GetAllRoles
{
    public class GetAllRolesQuery : IRequest<List<string>>
    {
    }
}
