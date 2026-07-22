using Abwaab.Application.DTOs.ApplicationUser;

namespace Abwaab.Application.Common.Contracts
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterDTO registerDTO);
        Task<LoginUserResponse> LoginUserAsync(LoginUserDTO loginUserDTO);
        Task<VerifyCodeResponse> VerifyUserAsync(VerifyCodeDTO verifyCodeDTO);
        Task<bool> IsUserExistsAsync(IdentifierDTO resendCodeDTO);
        Task<ForgotPasswordResponse> ForgotPasswordAsyn(ForgotPasswordDTO request);
    }
}
