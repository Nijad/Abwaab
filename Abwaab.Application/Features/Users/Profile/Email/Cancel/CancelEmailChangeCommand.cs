using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Email.Cancel
{
    public class CancelEmailChangeCommand : IRequest<CancelEmailChangeResponse>
    {
        // No properties needed – we get the user from the context
    }
}
