using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Application.DTOs.ApplicationUser.ChangePassword;
using Abwaab.Application.DTOs.ApplicationUser.ForgotPassword;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using Abwaab.Application.DTOs.ApplicationUser.LoginUser;
using Abwaab.Application.DTOs.ApplicationUser.LogoutUser;
using Abwaab.Application.DTOs.ApplicationUser.RegisterUser;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Common.Contracts
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDTO registerDTO);
        Task<LoginUserResponse> LoginUserAsync(LoginUserDTO loginUserDTO);
        Task<VerifyCodeResponse> VerifyUserAsync(VerifyCodeDTO verifyCodeDTO);
        Task<bool> IsUserExistsAsync(IdentifierDTO resendCodeDTO);
        Task<ForgotPasswordResponse> ForgotPasswordAsyn(ForgotPasswordDTO request);
        Task<ChangePasswordResponse> ChangePassword(ChangePasswordDTO request);
        Task<LogoutResponse> Logout(LogoutRequest request);

        Task<bool> MappingUserWithNotificationWay(ApplicationUser user, NotificationWayEnum notificationWayType);
        Task<InitiateEmailChangeResponse> InitiatieEmailChange(InitiateEmailChangeRequest request);
        Task<ConfirmEmailChangeResponse> ConfirmEmailChange(ConfirmEmailChangeRequest request);
        Task<InitiatePhoneNoChangeResponse> InitiatePhoneNoChange(InitiatePhoneNoChangeRequest request);
        Task<ConfirmPhoneNoChangeResponse> ConfirmPhoneNoChange(ConfirmPhoneNoChangeRequest request);
        Task<CancelEmailChangeResponse> CancelEmailChange(CancelEmailChangeRequest request);
        Task<CancelPhoneChangeResponse> CancelPhoneChange(CancelPhoneChangeRequest request);
    }
}
