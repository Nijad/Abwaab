using Abwaab.Application.DTOs.ApplicationUser.ChangePassword;
using Abwaab.Application.DTOs.ApplicationUser.ForgotPassword;
using Abwaab.Application.DTOs.ApplicationUser.IdentifierManagement;
using Abwaab.Application.DTOs.ApplicationUser.LoginUser;
using Abwaab.Application.DTOs.ApplicationUser.LogoutUser;
using Abwaab.Application.DTOs.ApplicationUser.RegisterUser;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using Abwaab.Application.DTOs.Roles.AddRoleToUser;
using Abwaab.Application.DTOs.Roles.GetUserRoles;
using Abwaab.Application.DTOs.Roles.RemoveUserFormRole;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Common.Contracts
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterUserCommandAsync(RegisterUserDTO registerDTO);
        Task<LoginUserResponse> LoginUserCommandAsync(LoginUserDTO loginUserDTO);
        Task<VerifyCodeResponse> VerifyUserCommandAsync(VerifyCodeDTO verifyCodeDTO);
        Task<bool> IsUserExistsCommandAsync(ResendCodeDTO resendCodeDTO);
        Task<ForgotPasswordResponse> ForgotPasswordCommandAsyn(ForgotPasswordDTO request);
        Task<ChangePasswordResponse> ChangePasswordCommandAsync(ChangePasswordDTO request);
        Task<LogoutResponse> LogoutCommandAsync(LogoutCommand request);
        Task<bool> MappingUserWithNotificationWayCommandAsync(ApplicationUser user, NotificationWayEnum notificationWayType);
        Task<InitiateEmailChangeResponse> InitiatieEmailChangeCommandAsync(InitiateEmailChangeCommand request);
        Task<ConfirmEmailChangeResponse> ConfirmEmailChangeCommandAsync(ConfirmEmailChangeCommand request);
        Task<InitiatePhoneNoChangeResponse> InitiatePhoneNoChangeCommandAsync(InitiatePhoneNoChangeCommand request);
        Task<ConfirmPhoneNoChangeResponse> ConfirmPhoneNoChangeCommandAsync(ConfirmPhoneNoChangeCommand request);
        Task<CancelEmailChangeResponse> CancelEmailChangeCommandAsync(CancelEmailChangeCommand request);
        Task<CancelPhoneChangeResponse> CancelPhoneChangeCommandAsync(CancelPhoneChangeCommand request);
        Task<AddUserToRoleResponse> AddUserToRoleCommandAsync(AddUserToRoleDTO request);
        Task<RemoveUserFromRoleResponse> RemoveUserFromRoleCommandAsync(RemoveUserFromRoleDTO request);
        Task<GetUserRolesResponse> GetUserRolesQueryAsync(GetUserRolesDTO request);
        Task<List<string>> GetAllRolesQueryAsync();
    }
}
