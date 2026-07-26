using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement
{
    public class CancelEmailChangeRequest : IRequest<CancelEmailChangeResponse>
    {
        // No properties needed – we get the user from the context
    }
}
