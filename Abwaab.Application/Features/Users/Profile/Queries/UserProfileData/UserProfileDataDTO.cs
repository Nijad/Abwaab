using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Queries.UserProfileData
{
    public class UserProfileDataDTO : IRequest<UserProfileDataResponse>
    {
        public string Identifier { get; set; } = string.Empty;
        public IdentifiersEnum IdentifierType { get; set; }
    }
}
