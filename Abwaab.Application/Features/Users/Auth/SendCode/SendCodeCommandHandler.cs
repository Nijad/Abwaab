using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Infrastructure.Common;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Auth.SendCode
{
    public class SendCodeCommandHandler : IRequestHandler<SendCodeDTO, SendCodeResponse>
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

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
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);

            if (user == null)
                throw new NotFoundException("User", nameof(request.IdentifierType), request.Identifier);

            string code = _verificationCodeService.GenerateVerificationCode();
            request.Code = code;
            
            var result = await _verificationCodeService.SendVerificationCodeAsync(request);
            
            // Store the code in cache with a 5-minute expiry
            _cache.Set(request.Identifier, code, TimeSpan.FromMinutes(Constants.CODE_TIMEOUT_MINUTES));

            return result;
        }
    }
}
