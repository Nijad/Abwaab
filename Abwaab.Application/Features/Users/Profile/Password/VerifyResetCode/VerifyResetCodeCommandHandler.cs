using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Profile.VerificationCode;
using Abwaab.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Profile.Password.VerifyResetCode
{
    public class VerifyResetCodeCommandHandler : IRequestHandler<VerifyResetCodeDTO, VerifyResetCodeResponse>
    {
        private readonly IMemoryCache _cache;
        private readonly string errorTitle = ErrorTitle.VerificationCode;

        public VerifyResetCodeCommandHandler(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<VerifyResetCodeResponse> Handle(VerifyResetCodeDTO request, CancellationToken cancellationToken)
        {
            var cacheKey = $"reset_{request.Identifier}";
            if (!_cache.TryGetValue(cacheKey, out string storedCode))
                if (request.IdentifierType == IdentifiersEnum.Email)
                    throw new InvalidCodeOrEmailMissmatchException(errorTitle);
                else if (request.IdentifierType == IdentifiersEnum.Phone_Number)
                    throw new InvalidCodeOrPhoneMissmatchException(errorTitle);
                else
                    throw new NotImplementedIdentifierException(request.IdentifierType.ToString().Replace("_", " "), errorTitle);

            if (storedCode != request.Code)
                    throw new InvalidVerificationCodeException(errorTitle);

            // Set a flag that code is verified
            _cache.Set($"reset_verified_{request.Identifier}", true, TimeSpan.FromMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES));

            return new VerifyResetCodeResponse { 
                Success = true, 
                Message = "Code verified.",
                CodeTimeOutInMinuts = GeneralConstants.CODE_TIMEOUT_MINUTES,
                ExpireAt = DateTime.UtcNow.AddMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES),
            };
        }
    }
}
