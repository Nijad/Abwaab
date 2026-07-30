using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Interfaces;
using MediatR;

namespace Abwaab.Application.Features.Users.Auth.SendCode
{
    public class SendCodeCommandHandler : IRequestHandler<SendCodeDTO, SendCodeResponse>
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IAuthService _authService;
        public SendCodeCommandHandler(IVerificationCodeService verificationCodeService, IAuthService authService)
        {
            _verificationCodeService = verificationCodeService;
            _authService = authService;
        }
        public async Task<SendCodeResponse> Handle(SendCodeDTO request, CancellationToken cancellationToken)
        {
            bool isUserExists = await _authService.IsUserExistsCommandAsync(request);

            if (!isUserExists)
            {
                return new SendCodeResponse
                {
                    IsSuccess = false,
                    Message = $"User with {request.IdentifierType.ToString().Replace('_', ' ')} {request.Identifier} does not exist."
                };
            }

            SendCodeResponse result = await _verificationCodeService.ResendVerificationCodeAsync(request);
            return result;
        }
    }
}
