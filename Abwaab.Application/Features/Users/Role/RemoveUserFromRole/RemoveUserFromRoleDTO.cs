using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Users.Role.RemoveUserFromRole
{
    public class RemoveUserFromRoleDTO : IRequest<RemoveUserFromRoleResponse>
    {
        public string Identifier { get; set; }
        public IdentifiersEnum IdentifierType { get; set; }
        public string RoleName { get; set; }
    }
}
