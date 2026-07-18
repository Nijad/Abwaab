using Abwaab.Application.DTOs.ApplicationUser;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Abwaab.Server.Validations.AuthValidations
{
    public class RegisterUserValidation : AbstractValidator<RegisterRequest>
    {
        public RegisterUserValidation()
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
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain a number.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MinimumLength(2).WithMessage("First Name must be at least 2 characters long.");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MinimumLength(2).WithMessage("Last Name must be at least 2 characters long.");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
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
