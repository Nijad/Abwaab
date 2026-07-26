using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.DTOs.Roles.AddRoleToUser
{
    public class AddUserToRoleDTO : IRequest<AddUserToRoleResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public IdentifierEnum IdentifierType { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
