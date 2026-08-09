namespace Abwaab.Application.Common.Constants
{
    public static class ErrorMessages
    {
        public const string EmailNotVerified = "Your email is not verified. Please Verify your email.";
        public const string PhoneNotVerified = "Your phone is not verified. Please Verify your phone no.";
        public const string AccountLocked = "Your account is locked. Go To forgot password";
        public const string InvalidCredentials = "Incorrect username or password";
        public const string UserAlreadyExist = "User is already exist";
        public const string RegistrationFailed = "Registration failed";
        public const string FailedSendingEmail = "Failed Sending email";
        public const string FailedSendingSms = "Failed Sending SMS";
        public const string InvalidVerificationCode = "Invalid or expired verification code.";
        public const string FailedConfirmationEmail = "Failed to confirm email";
        public const string FailedConfirmationPhone = "Failed to confirm phone";
        public const string InvalidRefreshToken = "Invalid or expired refresh token.";
        public const string NoPendingEmailChange = "No pending email change found.";
        public const string NoPendingPhoneChange = "No pending phone change found.";
        public const string InvalidCodeOrEmailMismatch = "Invalid code or email mismatch.";
        public const string InvalidCodeOrPhoneMissmatch = "Invalid code or phone mismatch.";
        public const string EmailAlreadyInUse = "Email is already in use by another account.";
        public const string PhoneAlreadyInUse = "Phone is already in use by another account.";
        public const string YourCurrentEmail = "This is already your current email.";
        public const string YourCurrentPhone = "This is already your current phone.";
        public const string NoRegisterdEmail = "You don't have email yet, please add email first.";
        public const string NoRegisterdPhone = "You don't have phone yet, please add phone first.";
        public const string NoVerifiedEmail = "Your email is not verified, please verify email first.";
        public const string NoVerifiedPhone = "Your phone is not verified, please verify phone first.";
        public const string FailedChangePassword = "Failed to change password.";
        public const string FailedToAddUserToRole = "Failed to add user to role.";
        public const string FailedToRemoveUserFromRole = "Failed to remove user from role.";
        public const string FailedResetPassword = "Failed to reset password.";
        public const string UserAlreadyHasPlan = "User already has a plan.";
        public const string PlanNotAvailable = "The selected plan is not available.";
        public const string UserAlreadyHasActivePlan = "User already has an active plan";
        public const string NotValidPaymentCode = "Payment code is not valid";
    }
}
