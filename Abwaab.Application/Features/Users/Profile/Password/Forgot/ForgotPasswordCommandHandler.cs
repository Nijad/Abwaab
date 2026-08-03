using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Abwaab.Infrastructure.Common;
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

            if (request.IdentifierType == IdentifierEnum.email)
                await _verificationService.SendVerificationCodeViaEmailAsync(user.Email!, code);
            else if (request.IdentifierType == IdentifierEnum.phone_number)
                await _verificationService.SendVerificationCodeViaSmsAsync(user.PhoneNumber!, code);
            else
                throw new NotImplementedIdentifierException(request.IdentifierType.ToString());

            _cache.Set($"reset_{request.Identifier}", code, TimeSpan.FromMinutes(Constants.CODE_TIMEOUT_MINUTES));

            return new ForgotPasswordResponse { Success = true, Message = $"Reset code sent to your {request.IdentifierType.ToString().Replace("_", " ")}." };
        }
    }
}
