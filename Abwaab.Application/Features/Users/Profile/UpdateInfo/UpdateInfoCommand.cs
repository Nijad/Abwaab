using MediatR;

namespace Abwaab.Application.Features.Users.Profile.UpdateInfo
{
    public class UpdateInfoCommand : IRequest<UpdateInfoResponse>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
