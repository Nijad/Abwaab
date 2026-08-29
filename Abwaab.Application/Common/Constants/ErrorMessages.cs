namespace Abwaab.Application.Common.Constants
{
    public static class ErrorMessages
    {
        public const string SystemError = "حدث خطأ في النظام، يرجى المحاولة لاحقاً.";
        public const string AccountLocked = "حسابك مقفل. انتقل إلى نسيت كلمة المرور.";
        public const string EmailNotVerified = "بريدك الإلكتروني غير مؤكد. يرجى تأكيد بريدك الإلكتروني.";
        public const string PhoneNotVerified = "رقم هاتفك غير مؤكد. يرجى تأكيد رقم هاتفك.";
        public const string InvalidCredentials = "اسم المستخدم أو كلمة المرور غير صحيحة.";
        public const string UserAlreadyExist = "المستخدم موجود بالفعل.";
        public const string RegistrationFailed = "فشل التسجيل.";
        public const string FailedSendingEmail = "فشل إرسال البريد الإلكتروني.";
        public const string FailedSendingSms = "فشل إرسال الرسائل النصية.";
        public const string InvalidVerificationCode = "رمز التحقق غير صالح أو منتهي الصلاحية.";
        public const string FailedConfirmationEmail = "فشل تأكيد البريد الإلكتروني.";
        public const string FailedConfirmationPhone = "فشل تأكيد رقم الهاتف.";
        public const string InvalidRefreshToken = "رمز التحديث غير صالح أو منتهي الصلاحية.";
        public const string NoPendingEmailChange = "لا يوجد طلب تغيير بريد إلكتروني معلق.";
        public const string NoPendingPhoneChange = "لا يوجد طلب تغيير رقم هاتف معلق.";
        public const string InvalidCodeOrEmailMismatch = "رمز غير صالح أو عدم تطابق البريد الإلكتروني.";
        public const string InvalidCodeOrPhoneMissmatch = "رمز غير صالح أو عدم تطابق رقم الهاتف.";
        public const string EmailAlreadyInUse = "البريد الإلكتروني قيد الاستخدام بالفعل من قبل حساب آخر.";
        public const string PhoneAlreadyInUse = "رقم الهاتف قيد الاستخدام بالفعل من قبل حساب آخر.";
        public const string YourCurrentEmail = "هذا هو بريدك الإلكتروني الحالي بالفعل.";
        public const string YourCurrentPhone = "هذا هو رقم هاتفك الحالي بالفعل.";
        public const string NoRegisterdEmail = "ليس لديك بريد إلكتروني مسجل حتى الآن، يرجى إضافة بريد إلكتروني أولاً.";
        public const string NoRegisterdPhone = "ليس لديك رقم هاتف مسجل حتى الآن، يرجى إضافة رقم هاتف أولاً.";
        public const string NoVerifiedEmail = "بريدك الإلكتروني غير مؤكد، يرجى تأكيد البريد الإلكتروني أولاً.";
        public const string NoVerifiedPhone = "رقم هاتفك غير مؤكد، يرجى تأكيد رقم الهاتف أولاً.";
        public const string FailedChangePassword = "فشل تغيير كلمة المرور.";
        public const string FailedToAddUserToRole = "فشل إضافة المستخدم إلى الدور.";
        public const string FailedToRemoveUserFromRole = "فشل إزالة المستخدم من الدور.";
        public const string FailedResetPassword = "فشل إعادة تعيين كلمة المرور.";
        public const string UserAlreadyHasPlan = "المستخدم لديه خطة بالفعل.";
        public const string PlanNotAvailable = "الخطة المحددة غير متوفرة.";
        public const string UserAlreadyHasActivePlan = "المستخدم لديه خطة نشطة بالفعل.";
        public const string NotValidPaymentCode = "رمز الدفع غير صالح.";
        public const string UserHasNoActivePlan = "يجب أن يكون لدى المستخدم خطة نشطة واحدة.";
        public const string UserHasMoreThanOneActivePlan = "يجب أن يكون لدى المستخدم خطة نشطة واحدة فقط.";
        public const string UpdateUserFailed = "فشل تعديل بيانات المستخدم.";
        public const string PropertyNotFound = "العقار المطلوب غير موجود";
        public const string PropertyTypeNotFound = "نوع العقار المطلوب غير موجود";
        public const string PropertyFinishingNotFound = "كسوة العقار المطلوبة غير موجودة";
        public const string TimeSlotNotFound = "الفترة الزمنية التي تطلبها غير موجودة";
        public const string PropertyAttributeNotFound = "ميزة العقار التي تطلبها غير موجودة";
        public const string AttributeNotFound = "الميزة المطلبوة غير موجودة.";
        public const string DataTypeNotImplemented = "نوع البيانات المطلوب غير منجز.";
        public const string NotValidNumber = "القيمة يجب أن تكون رقماً صحيحاً موجباً.";
        public const string NotValidBoolean = "القيمة يجب أن تكون صح أو خطأ.";
        public const string NoNullAllowed = "القيمة المدخلة يجب ألا تكون فارغة.";
        public const string NotValidFormat = "القيم المدخلة لا تتطابق مع التنسيق الصحيح";
        public const string AttributePossibleValueNotFound = "قيمة ميزة العقار المطلوبة غير موجودة ضمن القيم المعرفة مسبقاً";
        public const string UserNotAuthenticated = "يجب تسجيل دخول المستخدم.";
        public const string MediaNotFound = "ملف الوسائط المطلوب غير موجود.";
        public const string PropertyAlreadyStared = "العقار تم تمييزه مسبقاً.";
        public const string PropertyAlreadyUnstared = "العقار تم إلغاء تمييزه مسبقاً.";
        public const string HasNoCoverImage = "صورة غلاف العقار مطلوبة.";
        public const string AppointmentStateNotFound = "حالة الموعد المطلوبة غير موجودة.";
        public const string NoTimeSlotsConfigured = "لم يتم تحديد فترات زمنية للعقار.";
        public const string SameOwner = "لا يمكنك حجز موعد لهذا العقار، حيث أنه يعود إليك.";
        public const string NotPublishedProperty = "لا يمكنك حجز موعد لهذا العقار، حيث أن العقار غير متاح حالياً.";
        public const string TimeSlotNotAvailable = "الوقت الذي اخترته لحجز موعد زيارة للعقار لم يعد متاحاً، يرجى اختيار وقت آخر.";
    }
}