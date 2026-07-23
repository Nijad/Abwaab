using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Application.DTOs.ApplicationUser.ForgotPassword;
using Abwaab.Application.DTOs.ApplicationUser.LoginUser;
using Abwaab.Application.DTOs.ApplicationUser.RegisterUser;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;

namespace Abwaab.Application.Common.Contracts
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDTO registerDTO);
        Task<LoginUserResponse> LoginUserAsync(LoginUserDTO loginUserDTO);
        Task<VerifyCodeResponse> VerifyUserAsync(VerifyCodeDTO verifyCodeDTO);
        Task<bool> IsUserExistsAsync(IdentifierDTO resendCodeDTO);
        Task<ForgotPasswordResponse> ForgotPasswordAsyn(ForgotPasswordDTO request);
    }
}
