using Abwaab.Application.Common.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
    }
}
