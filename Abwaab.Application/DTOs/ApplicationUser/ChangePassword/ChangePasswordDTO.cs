using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.ChangePassword
{
    public class ChangePasswordDTO : IRequest<ChangePasswordResponse>
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
