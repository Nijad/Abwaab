using Abwaab.Application.DTOs.ApplicationUser;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class LoginUserValidation : AbstractValidator<LoginUserRequest>
    {
        public LoginUserValidation()
        {
            RuleFor(x => x)
           .Must(x => !string.IsNullOrEmpty(x.Email) || !string.IsNullOrEmpty(x.PhoneNo))
           .WithMessage("You must provide either an Email or a Phone Number.");

            RuleFor(x => x.Email)
                .Must(email => string.IsNullOrEmpty(email) || new EmailAddressAttribute().IsValid(email))
                .WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNo)
            .Must(IsValidPhoneNumber)
            .WithMessage("Phone Number must be in international format +9639XXXXXXXX");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return true;
            // Implement your phone number validation logic here
            // For example, you can use a regular expression to validate the format
            var regex = new Regex(@"\+9639[0-9]{8}");
            return regex.IsMatch(phoneNumber);
        }
    }
}
