using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.DTOs.Roles.RemoveUserFormRole
{
    public class RemoveUserFromRoleDTO : IRequest<RemoveUserFromRoleResponse>
    {
        public string Identifier { get; set; }
        public IdentifierEnum IdentifierType { get; set; }
        public string RoleName { get; set; }
    }
}
