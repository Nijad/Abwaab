using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Auth.SendCode
{
    public class SendCodeCommandHandler : IRequestHandler<SendCodeDTO, SendCodeResponse>
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;
        private readonly string errorTitle = ErrorTitle.SendCode;

        public SendCodeCommandHandler(
            IVerificationCodeService verificationCodeService,
            IUserService userService,
            IMemoryCache cache)
        {
            _verificationCodeService = verificationCodeService;
            _userService = userService;
            _cache = cache;
        }
        public async Task<SendCodeResponse> Handle(SendCodeDTO request, CancellationToken cancellationToken)
        {
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType, errorTitle);

            if (user == null)
                throw new UserNotFoundException(request.Identifier, errorTitle);

            string cooldownKey = $"resend_cooldown_{request.Identifier}";
            if (_cache.TryGetValue(cooldownKey, out _))
                throw new ResendWaitException();

            string code = _verificationCodeService.GenerateVerificationCode();
            request.Code = code;

            var result = await _verificationCodeService.SendVerificationCodeAsync(request);
            
            // Store the code in cache with a 5-minute expiry
            _cache.Set(request.Identifier, code, TimeSpan.FromMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES));

            _cache.Set($"resend_cooldown_{request.Identifier}", code, TimeSpan.FromMinutes(GeneralConstants.WAIT_TIMEOUT_MINUTES));

            result.ExpireAt = DateTime.UtcNow.AddMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES);
            result.CodeTimeOutInMinuts = GeneralConstants.CODE_TIMEOUT_MINUTES;
            return result;
        }
    }
}
