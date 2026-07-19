using Abwaab.Application.DTOs.ApplicationUser;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class VerifyCodeValidation : AbstractValidator<VerifyCodeRequest>
    {
        public VerifyCodeValidation()
        {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.Email) || !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("You must provide either an Email or a Phone Number.");
            
            RuleFor(x => x.Email)
                .Must(email => string.IsNullOrEmpty(email) || new EmailAddressAttribute().IsValid(email))
                .WithMessage("Invalid email format.");
            
            RuleFor(x => x.PhoneNumber)
                .Must(IsValidPhoneNumber)
                .WithMessage("Phone Number must be in international format +9639XXXXXXXX");
            
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Verification code is required.")
                .Length(6).WithMessage("Verification code must be 6 digits long.");

            RuleFor(x=>x.Code)
                .Matches("^[0-9]{6}$").WithMessage("Verification code must be numeric."); 
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return true;
            var regex = new Regex(@"\+9639[0-9]{8}");
            return regex.IsMatch(phoneNumber);
        }
    }
}
