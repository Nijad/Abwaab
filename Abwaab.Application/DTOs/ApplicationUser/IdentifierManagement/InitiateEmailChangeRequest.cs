using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement
{
    public class InitiateEmailChangeRequest : IRequest<InitiateEmailChangeResponse>
    {
        public string NewEmail { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
    }
}
