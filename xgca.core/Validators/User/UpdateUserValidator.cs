using FluentValidation;
using System.Collections;
using System.Collections.Generic;
using xgca.core.Models.User;
using IUserData = xgca.data.User.IUserData;

namespace xgca.core.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserModel>
    {
        public UpdateUserValidator(IUserData userData)
        {
            // Checking of required fields
            RuleFor(o => o.FirstName).NotEmpty().WithMessage("Firstname is required.");
            RuleFor(o => o.LastName).NotEmpty().WithMessage("Lastname is required.");
            RuleFor(o => o.MobilePrefixId).NotEmpty().WithMessage("Mobile prefix is required.");
            RuleFor(o => o.MobilePrefix).NotEmpty().WithMessage("Mobile prefix is required.");
            RuleFor(o => o.Mobile).NotEmpty().WithMessage("Mobile number is required.");
            RuleFor(o => o.PhonePrefixId).NotEmpty().WithMessage("Phone prefix is required.");
            RuleFor(o => o.PhonePrefix).NotEmpty().WithMessage("Phone prefix is required.");
            RuleFor(o => o.Phone).NotEmpty().WithMessage("Phone number is required.");
            RuleFor(o => o.Title).NotEmpty().WithMessage("Title/Designation is required.");
            //RuleFor(o => o.EmailAddress).NotEmpty().WithMessage("EmailAddress is required.");
        }
    }
}
