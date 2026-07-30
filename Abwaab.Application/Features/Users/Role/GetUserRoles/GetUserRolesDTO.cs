using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Users.Role.GetUserRoles
{
    public class GetUserRolesDTO : IRequest<GetUserRolesResponse>
    {
        public string Identifier { get; set; }
        public IdentifierEnum IdentifierType { get; set; }
    }
}
