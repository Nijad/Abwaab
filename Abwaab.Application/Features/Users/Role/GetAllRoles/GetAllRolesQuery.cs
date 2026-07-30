using MediatR;

namespace Abwaab.Application.Features.Users.Role.GetAllRoles
{
    public class GetAllRolesQuery : IRequest<List<string>>
    {
    }
}
