using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Plans;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.PaymentEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Abwaab.Application.Features.Users.Profile.UserPlans.Cancel
{
    public class CancelUserPlanCommandHandler : IRequestHandler<CancelUserPlanCommand, CancelUserPlanResponse>
    {
        private readonly IPlanService _planService;
        private readonly IUserPlanStateService _userPlanStateService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClaimsPrincipal currentUser;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly ITransactionManager _transactionManager;
        private readonly string errorTitle = ErrorTitle.CancelUserPlan;

        public CancelUserPlanCommandHandler(
            IPlanService planService,
            IHttpContextAccessor contextAccessor,
            IUserService userService,
            IPaymentService paymentService,
            ITransactionManager transactionManager,
            IUserPlanStateService userPlanStateService)
        {
            _planService = planService;
            _contextAccessor = contextAccessor;
            currentUser = _contextAccessor.HttpContext.User;
            _userService = userService;
            _paymentService = paymentService;
            _transactionManager = transactionManager;
            _userPlanStateService = userPlanStateService;
        }

        public async Task<CancelUserPlanResponse> Handle(CancelUserPlanCommand request, CancellationToken cancellationToken)
        {
            await _transactionManager.BeginTransactionAsync(cancellationToken);
            try
            {
                // 1. check if there is a user plan with id equal to userplanId
                UserPlan? userPlan = await _planService.FindUserPlanByIdAsync(request.UserPlanId);
                if (userPlan == null)
                    throw new NotFoundException(nameof(UserPlan), nameof(request.UserPlanId), request.UserPlanId.ToString(), errorTitle);

                // 2. check if a plan belong to user or user is an admin
                string username = currentUser.Identity!.Name!;
                ApplicationUser? user = await _userService.FindUserByNameAsync(username);
                if (user == null)
                    throw new UserNotFoundException(username, errorTitle);

                if (userPlan.UserId != user.Id && !currentUser.IsInRole(RoleConstants.ROLE_ADMIN))
                    throw new ObjectNotBelongToUserException(nameof(UserPlan),errorTitle);

                // 3. check if user plan with pending status
                UserPlanStatus activeStatus = await _userPlanStateService.GetActiveUserPlanStatus(errorTitle);
                if (activeStatus.Id != userPlan.UserPlanStateId)
                    throw new FailedCancellationUserPlanException($"لا يمكنك إلغاء الاشتراك بهذه الخطة، لقد قمت بتثبيتها مسبقاً.", errorTitle);

                // 4. change status of user plan to canceled

                UserPlanStatus canceledStatus = await _userPlanStateService.GetCanceledUserPlanStatus(errorTitle);

                userPlan.UserPlanStatus = canceledStatus;
                userPlan.UserPlanStateId = canceledStatus.Id;
                userPlan.LastModifiedAt = DateTime.Now;
                userPlan.LastModifiedBy = username;

                await _planService.UpdateUserPlan(userPlan);

                // 5. cancel payment of this user plan
                PaymentState pendingPaymentState = await _paymentService.FindPaymentSateBySateNameAsync(PaymentStatesEnum.Pending, errorTitle);
                PaymentState canceledPaymentState = await _paymentService.FindPaymentSateBySateNameAsync(PaymentStatesEnum.Cancelled, errorTitle);

                Payment? userPlanPendingPayment = await _paymentService.FindPendingUserPlanPaymentAsync(userPlan.Id, errorTitle);

                if (userPlanPendingPayment != null)
                {
                    userPlanPendingPayment.PaymentState = pendingPaymentState;
                    await _paymentService.UpdatePaymentAsync(userPlanPendingPayment);
                }

                await _transactionManager.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _transactionManager.RollbackTransactionAsync(cancellationToken);
                throw;
            }
            return new CancelUserPlanResponse() { Success = true, Message = "" };
        }
    }
}
