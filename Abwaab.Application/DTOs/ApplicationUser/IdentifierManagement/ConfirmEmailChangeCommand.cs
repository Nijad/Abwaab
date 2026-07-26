using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement
{
    public class ConfirmEmailChangeCommand : IRequest<ConfirmEmailChangeResponse>
    {
        public string NewEmail { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
