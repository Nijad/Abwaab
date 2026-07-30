using Abwaab.Application.Common.Interfaces;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Password.Change
{
    public class ChangePasswordDTO : IRequest<ChangePasswordResponse>, IUserRequest
    {
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
