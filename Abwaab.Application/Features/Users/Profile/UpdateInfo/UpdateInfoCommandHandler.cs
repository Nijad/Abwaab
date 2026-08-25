using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Profile.UpdateInfo
{
    public class UpdateInfoCommandHandler : IRequestHandler<UpdateInfoCommand, UpdateInfoResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly ILogger<UpdateInfoCommandHandler> _logger;
        private readonly string errorTitle = ErrorTitle.UpdateUser;

        public UpdateInfoCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            ILogger<UpdateInfoCommandHandler> logger)
        {
            _userManager = userManager;
            _userService = userService;
            _logger = logger;
        }

        public async Task<UpdateInfoResponse> Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
        {
            string username = _userService.FindUserNameByContext(errorTitle);
            
            ApplicationUser? user = await _userManager.FindByNameAsync(username);

            if(user == null)
                throw new UserNotFoundException(username, errorTitle);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                _logger.LogError("Failed to register user. Errors: {Errors}", errors);

                throw new UpdateUserFailedException(errorTitle);
            }

            return new UpdateInfoResponse() {  Success = true, Message = "تم تعديل بيانات المستخدم بنجاح."};
        }
    }
}
