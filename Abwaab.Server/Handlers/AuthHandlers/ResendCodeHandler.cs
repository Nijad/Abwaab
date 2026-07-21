using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using MediatR;

namespace Abwaab.Server.Handlers.AuthHandlers
{
    public class ResendCodeHandler : IRequestHandler<ResendCodeDTO, ResendCodeResponse>
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IAuthService _authService;
        public ResendCodeHandler(IVerificationCodeService verificationCodeService, IAuthService authService)
        {
            _verificationCodeService = verificationCodeService;
            _authService = authService;
        }
        public async Task<ResendCodeResponse> Handle(ResendCodeDTO request, CancellationToken cancellationToken)
        {
            bool isUserExists = await _authService.IsUserExistsAsync(request);

            if (!isUserExists)
            {
                return new ResendCodeResponse
                {
                    IsSuccess = false,
                    Message = $"User with {request.IdentifierType.ToString().Replace('_', ' ')} {request.Identifier} does not exist."
                };
            }

            ResendCodeResponse result = await _verificationCodeService.ResendVerificationCodeAsync(request.Identifier);
            return result;
        }
    }
}
