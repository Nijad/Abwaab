using Abwaab.Application.Common.Constants;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Presistence.Seeding
{
    public class AppDbContextInitializer : IAppDbContextInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<AppDbContextInitializer> _logger;

        public AppDbContextInitializer(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<AppDbContextInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Applies pending migrations and creates the database if it doesn't exist
                if (_context.Database.IsRelational())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            // 1. Seed Roles
            var adminRole = new ApplicationRole { Id = new Guid(), Name = RoleConstants.ROLE_ADMIN, NormalizedName = RoleConstants.ROLE_ADMIN.ToUpper() };
            var userRole = new ApplicationRole { Id = new Guid(), Name = RoleConstants.ROLE_USER, NormalizedName = RoleConstants.ROLE_USER.ToUpper() };

            if (!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(adminRole);
                await _roleManager.CreateAsync(userRole);
            }

            // 2. Seed Admin User
            var adminUser = new ApplicationUser
            {
                UserName = "admin@abwaab.com",
                Email = "admin@abwaab.com",
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            if (await _userManager.Users.AllAsync(u => u.UserName != adminUser.UserName))
            {
                // Password hashing happens safely at runtime here
                var result = await _userManager.CreateAsync(adminUser, "SecurePassword123!");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
                }
            }

            if (!await _context.MediaTypes.AnyAsync())
                await _context.MediaTypes.AddRangeAsync(SeedData.LoadMediaTypes());

            if (!await _context.ServiceTypes.AnyAsync())
                await _context.ServiceTypes.AddRangeAsync(SeedData.LoadServiceTypes());

            if (!await _context.PaymentStates.AnyAsync())
                await _context.PaymentStates.AddRangeAsync(SeedData.LoadPaymentStates());

            if (!await _context.AppointmentStates.AnyAsync())
                await _context.AppointmentStates.AddRangeAsync(SeedData.LoadAppointmentStates());

            if (!await _context.NotificationStates.AnyAsync())
                await _context.NotificationStates.AddRangeAsync(SeedData.LoadNotificationStates());

            if (!await _context.NotificationWays.AnyAsync())
                await _context.NotificationWays.AddRangeAsync(SeedData.LoadNotificationWays());

            if (!await _context.AppointmentActions.AnyAsync())
                await _context.AppointmentActions.AddRangeAsync(SeedData.LoadAppointmentActions());

            if (!await _context.PropertyStates.AnyAsync())
                await _context.PropertyStates.AddRangeAsync(SeedData.LoadPropertyStates());

            if (!await _context.Plans.AnyAsync())
                await _context.AddRangeAsync(SeedData.LoadPlans());

            if (!await _context.Finishings.AnyAsync())
                await _context.AddRangeAsync(SeedData.LoadFinishings());

            if (!await _context.PropertyTypes.AnyAsync())
                await _context.AddRangeAsync(SeedData.LoadPropertyTypes());

            if (!await _context.AttributeDataTypes.AnyAsync())
                await _context.AddRangeAsync(SeedData.LoadAttributeDataTypeValues());

            if (!await _context.Attributes.AnyAsync())
                await _context.AddRangeAsync(SeedData.LoadAttributes());

            //if (!await _context.AttributePossibleValues.AnyAsync())
            //    await _context.AddRangeAsync(SeedData.LoadAttributePossibleValues());

            if (!await _context.UserPlansStatus.AnyAsync())
                await _context.AddRangeAsync(SeedData.LoadUserPlanStates());

            await _context.SaveChangesAsync();
        }
    }
}
