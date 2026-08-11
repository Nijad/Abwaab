using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Profile.Password.Forgot
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordDTO, ForgotPasswordResponse>
    {
        private readonly IVerificationCodeService _verificationService;
        private readonly IMemoryCache _cache;
        private readonly IUserService _userService;

        public ForgotPasswordCommandHandler(
            IVerificationCodeService verificationService,
            IMemoryCache cache,
            IUserService userService)
        {
            _verificationService = verificationService;
            _cache = cache;
            _userService = userService;
        }

        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordDTO request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                throw new NotFoundException("User", nameof(request.Identifier), request.Identifier);

            string code = _verificationService.GenerateVerificationCode();

            if (request.Identifier == user.Email)
                await _verificationService.SendVerificationCodeViaEmailAsync(user.Email!, code);
            else if (request.Identifier == user.PreviousEmail)
                await _verificationService.SendVerificationCodeViaEmailAsync(user.PreviousEmail!, code);
            else if (request.Identifier == user.PhoneNumber)
                await _verificationService.SendVerificationCodeViaSmsAsync(user.PhoneNumber!, code);
            else if (request.Identifier == user.PreviousPhoneNumber)
                await _verificationService.SendVerificationCodeViaSmsAsync(user.PreviousPhoneNumber!, code);
            else if (request.IdentifierType == IdentifiersEnum.Email)
                throw new NotFoundException(nameof(request.Identifier), IdentifiersEnum.Email.ToString(), request.Identifier);
            else if (request.IdentifierType == IdentifiersEnum.Phone_Number)
                throw new NotFoundException(nameof(request.Identifier), IdentifiersEnum.Phone_Number.ToString().Replace("_", " "), request.Identifier);
            else
                throw new NotImplementedIdentifierException(request.IdentifierType.ToString());

            _cache.Set($"reset_{request.Identifier}", code, TimeSpan.FromMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES));

            return new ForgotPasswordResponse { 
                Success = true, 
                Message = $"Reset code sent to your {request.IdentifierType.ToString().Replace("_", " ")}." ,
                CodeTimeOutInMinuts = GeneralConstants.CODE_TIMEOUT_MINUTES,
                ExpireAt = DateTime.UtcNow.AddMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES),
            };
        }
    }
}
