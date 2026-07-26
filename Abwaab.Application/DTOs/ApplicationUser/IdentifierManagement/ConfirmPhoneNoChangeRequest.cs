using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement
{
    public class ConfirmPhoneNoChangeRequest : IRequest<ConfirmPhoneNoChangeResponse>
    {
        public string NewPhoneNo { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
