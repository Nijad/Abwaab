using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Plans;
using Abwaab.Application.Common.Exceptions.Profile.Plans;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Profile.UserPlans.Upgrade
{
    public class UpgradeUserPlanCommandHandler : IRequestHandler<UpgradeUserPlanComman, UpgradeUserPlanResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPlanRepository _planRepository;
        private readonly IUserService _userService;
        private readonly ITransactionManager _transactionManager;
        private readonly IPaymentService _paymentService;
        private readonly IPlanService _planService;
        private readonly string errorTitle = ErrorTitle.UpgradeUserPlan;

        public UpgradeUserPlanCommandHandler(UserManager<ApplicationUser> userManager, IPlanRepository planRepository, IUserService userService, ITransactionManager transactionManager, IPaymentService paymentService, IPlanService planService)
        {
            _userManager = userManager;
            _planRepository = planRepository;
            _userService = userService;
            _transactionManager = transactionManager;
            _paymentService = paymentService;
            _planService = planService;
        }

        public async Task<UpgradeUserPlanResponse> Handle(UpgradeUserPlanComman request, CancellationToken cancellationToken)
        {
            await _transactionManager.BeginTransactionAsync(cancellationToken);
            try
            {
                string? username = _userService.FindUserNameByContext(errorTitle);
                if (username == null)
                    throw new NotFoundException(
                        entity: "user context",
                        property: "username",
                        value: "",
                        title: errorTitle);

                ApplicationUser? user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    throw new UserNotFoundException(username, errorTitle);

                Plan? plan = await _planRepository.GetPlanByIdAsync(request.PlanId);

                if (plan == null)
                    throw new PlanNotAvailableException(errorTitle);

                // check if plan is disabled or expired
                if (plan.IsDisabled || plan.ExpieryDate < DateOnly.FromDateTime(DateTime.UtcNow))
                    throw new PlanNotAvailableException(errorTitle);

                // check if the user already has the plan
                bool userAlreadyHasPlan = await _planRepository.UserHasPlanAsync(user.Id, plan.Id, errorTitle);
                if (userAlreadyHasPlan)
                    throw new UserAlreadyHasPlanException(errorTitle);

                PaymentState pendingPaymentState = await     _paymentService.FindPaymentSateBySateNameAsync(PaymentStatesEnum.Pending, errorTitle);

                ServiceType serviceType = await  _paymentService.FindServicTypeByNameAsync(ServiceTypesEnum.Plan_Subscription, errorTitle);

                UserPlanStatus? pendingUserPlanStatus = await _planService.FindUserPlanStatusByNameAsync(UserPlanStatesEnum.Pending, errorTitle);

                UserPlan userPlan = new()
                {
                    Id = new Guid(),
                    User = user,
                    UserId = user.Id,
                    Plan = plan,
                    PlanId = plan.Id,
                    UserPlanStatus = pendingUserPlanStatus,
                    UserPlanStateId = pendingUserPlanStatus.Id,
                    SubscriptionDate = DateOnly.FromDateTime(DateTime.Today),
                };
                await _planService.AddUserPlanAsync(userPlan);

                Payment payment = new Payment()
                {
                    Id = new Guid(),
                    Amount = plan.Price,
                    Description = $"دفعة لترقية اشتراك المستخدم في الخطة '{plan.Name}'",
                    PaymentCode = Guid.NewGuid().ToString(),
                    PaymentState = pendingPaymentState,
                    PaymentStateId = pendingPaymentState.Id,
                    ServiceType = serviceType,
                    ServiceTypeId = serviceType.Id
                };
                await _paymentService.AddPaymentAsync(payment);

                await _transactionManager.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _transactionManager.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            return new UpgradeUserPlanResponse
            {
                Success = true,
                Message = "تم ترقية اشتراكك بنجاح"
            };
        }
    }
}
