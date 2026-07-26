using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using MediatR;

namespace Abwaab.Server.Handlers.CodeHandlers
{
    public class ResendCodeCommandHandler : IRequestHandler<ResendCodeDTO, ResendCodeResponse>
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IAuthService _authService;
        public ResendCodeCommandHandler(IVerificationCodeService verificationCodeService, IAuthService authService)
        {
            _verificationCodeService = verificationCodeService;
            _authService = authService;
        }
        public async Task<ResendCodeResponse> Handle(ResendCodeDTO request, CancellationToken cancellationToken)
        {
            bool isUserExists = await _authService.IsUserExistsCommandAsync(request);

            if (!isUserExists)
            {
                return new ResendCodeResponse
                {
                    IsSuccess = false,
                    Message = $"User with {request.IdentifierType.ToString().Replace('_', ' ')} {request.Identifier} does not exist."
                };
            }

            ResendCodeResponse result = await _verificationCodeService.ResendVerificationCodeAsync(request);
            return result;
        }
    }
}
