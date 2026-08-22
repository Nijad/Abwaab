using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Email.Cancel
{
    public class CancelEmailChangeCommand : IRequest<CancelEmailChangeResponse>
    {
        public string ChangingCode { get; set; } = string.Empty;
    }
}
