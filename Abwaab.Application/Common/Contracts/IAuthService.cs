using Abwaab.Application.Features.Users.Auth.Login;
using Abwaab.Application.Features.Users.Auth.Logout;
using Abwaab.Application.Features.Users.Auth.Register;
using Abwaab.Application.Features.Users.Auth.SendCode;
using Abwaab.Application.Features.Users.Auth.VerificationCode;

namespace Abwaab.Application.Common.Contracts
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterUserCommandAsync(RegisterUserDTO registerDTO);
        Task<LoginUserResponse> LoginUserCommandAsync(LoginUserDTO loginUserDTO);
        Task<VerifyCodeResponse> VerifyUserCommandAsync(VerifyCodeDTO verifyCodeDTO);
        Task<bool> IsUserExistsCommandAsync(SendCodeDTO resendCodeDTO);        
        Task<LogoutResponse> LogoutCommandAsync(LogoutCommand request);
    }
}
