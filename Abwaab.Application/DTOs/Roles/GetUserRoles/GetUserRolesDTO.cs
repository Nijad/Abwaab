using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.DTOs.Roles.GetUserRoles
{
    public class GetUserRolesDTO : IRequest<GetUserRolesResponse>
    {
        public string Identifier { get; set; }
        public IdentifierEnum IdentifierType { get; set; }
    }
}
