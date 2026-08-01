using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Auth.SendCode;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> FindUserByIdentifierAsync(string identifier, IdentifierEnum identifierType)
        {
            ApplicationUser? user = null;
            if (identifierType == IdentifierEnum.email)
            {
                user = await _userManager.FindByEmailAsync(identifier);
                if (user != null)
                    return user;
                return await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PreviousEmail == identifier);
            }
            else if (identifierType == IdentifierEnum.phone_number)
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == identifier);
                if (user != null)
                    return user;
                return await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PreviousPhoneNumber == identifier);
            }

            throw new NotImplementdIdentifierException(identifierType.ToString());
        }
    }
}
