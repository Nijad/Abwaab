using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Phone.Cancel
{
    public class CancelPhoneChangeCommand : IRequest<CancelPhoneChangeResponse>
    {
        public string ChangingCode { get; set; } = string.Empty;
    }
}
