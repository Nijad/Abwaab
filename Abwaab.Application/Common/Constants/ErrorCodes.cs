namespace Abwaab.Application.Common.Constants
{
    public static class ErrorCodes
    {
        public const string EmailNotVerified  = "EMAIL_NOT_VERIFIED";
        public const string PhoneNotVerified = "PHONE_NOT_VERIFIED";
        public const string AccountLocked = "ACCOUNT_LOCKED";
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string UserAlreadyExist = "USER_ALREADY_EXIST";
        public const string RegistrationFailed = "REGISTRATION_FAILED";
        public const string FailedSendingEmail = "FAILED_SENDING_EMAIL";
        public const string FailedSendingSms = "FAILED_SENDING_SMS";
        public const string InvalidVerificationCode = "INVALID_VERIFICATION_CODE";
        public const string FailedConfirmationEmail = "FAILED_CONFIRMATION_EMAIL";
        public const string FailedConfirmationPhone = "FAILED_CONFIRMATION_PHONE";
        public const string NotImplementdIdentifier = "NOT_IMPLEMENTED_IDENTIFIER";
        public const string InvalidRefreshToken = "INVALID_REFRESH_TOKEN";
        public const string NoPendingEmailChange = "NO_PENDING_EMAIL_CHANGE";
        public const string NoPendingPhoneChange = "NO_PENDING_PHONE_CHANGE";
        public const string InvalidCodeOrEmailMismatch = "INVALID_CODE_OR_EMAIL_MISSMATCH";
        public const string InvalidCodeOrPhoneMissmatch = "INVALID_CODE_OR_PHONE_MISSMATCH";
        public const string EmailAlreadyInUse = "EMAIL_ALREADY_IN_USE";
        public const string PhoneAlreadyInUse = "PHONE_ALREADY_IN_USE";
        public const string YourCurrentEmail = "YOUR_CURRENT_EMAIL";
        public const string YourCurrentPhone = "YOUR_CURRENT_PHONE";
        public const string AlreadySubscribeNotificationWay = "ALREADY_SUBSCRIBE_NOTIFICATION_WAY";
        public const string AlreadyUnsubscribeNotificationWay = "ALREADY_UNSUBSCRIBE_NOTIFICATION_WAY";
        public const string NoRegisterdEmail = "NO_REGISTERD_EMAIL";
        public const string NoRegisterdPhone = "NO_REGISTERD_PHONE";
        public const string NoVerifiedEmail = "NO_VERIFIED_EMAIL";
        public const string NoVerifiedPhone = "NO_VERIFIED_PHONE";
        public const string FailedChangePassword = "FAILED_CHANGE_PASSWORD";
        public const string UserAlreadyInRole = "USER_ALREADY_IN_ROLE";
        public const string UserNotInRole = "USER_NOT_IN_ROLE";
        public const string FailedToAddUserToRole = "FAILED_TO_ADD_USER_TO_ROLE";
        public const string FailedToRemoveUserFromRole = "FAILED_TO_REMOVE_USER_FROM_ROLE";
        public const string FailedResetPassword = "FAILED_RESET_PASSWORD";
        public const string UserAlreadyHasPlan = "USER_ALREADY_HAS_PLAN";
        public const string PlanNotAvailable = "PLAN_NOT_AVAILABLE";
        public const string UserAlreadyHasActivePlan = "USER_ALREADY_HAS_ACTIVE_PLAN";
        public const string NotImplementedServiceType = "NOT_IMPLEMENTED_SERVICE_TYPE";
        public const string ObjectNotBelongToUser = "OBJECT_NOT_BELONG_TO_USER";
        public const string FailedCancelationUserPlan = "FAILED_CANCELATION_USER_PLAN";
        public const string NotValidPaymentCode = "NON_PAYABLE";
        public const string UserHasNoActivePlan = "USER_HAS_NO_ACTIVE_PLAN";
        public const string UserHasMoreThanOneActivePlan = "USER_HAS_MORE_THAN_ONE_ACATIVE_PLAN";
        public const string ExceededAllowedNumber = "EXCEEDED_ALLOWED_NUMBER";
        public const string NotFound = "NOT_FOUND";
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string ResendWait = "RESEND_WAIT";
        public const string UpdateUserFailed = "UPDATE_USER_FAILED";
        public const string PropertyNotFound = "PROPERTY_NOT_FOUND";
    }
}
