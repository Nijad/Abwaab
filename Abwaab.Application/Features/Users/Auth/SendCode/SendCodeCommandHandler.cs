using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using MediatR;

namespace Abwaab.Application.Features.Users.Auth.SendCode
{
    public class SendCodeCommandHandler : IRequestHandler<SendCodeDTO, SendCodeResponse>
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IUserService _userService;

        public SendCodeCommandHandler(
            IVerificationCodeService verificationCodeService, 
            IUserService userService)
        {
            _verificationCodeService = verificationCodeService;
            _userService = userService;
        }
        public async Task<SendCodeResponse> Handle(SendCodeDTO request, CancellationToken cancellationToken)
        {
            var user = _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);

            if(user == null)
                throw new NotFoundException("User", nameof(request.IdentifierType), request.Identifier);
            
            return await _verificationCodeService.SendVerificationCodeAsync(request);
        }
    }
}
