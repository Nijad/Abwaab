using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement
{
    public class InitiatePhoneNoChangeCommand : IRequest<InitiatePhoneNoChangeResponse>
    {
        public string NewPhoneNo { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
    }
}
