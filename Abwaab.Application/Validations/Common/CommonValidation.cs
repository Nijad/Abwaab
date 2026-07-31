using System.Net.Mail;

namespace Abwaab.Application.Validations.Common
{
    public static class CommonValidation
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            // Simple regex for international phone numbers
            var regex = new System.Text.RegularExpressions.Regex(@"^\+9639\d{8}$");
            return regex.IsMatch(phoneNumber);
        }

        public static bool IsEmailOrPhoneNo(string identifier)
        {
            if (IsValidEmail(identifier) || IsValidPhoneNumber(identifier))
                return true;
            return false;
        }
    }
}
