using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using xas.core.Request;
using xas.data.accreditation.Request;

namespace xgca.core.Validators.Request
{
    public class CreateRequestValidator : AbstractValidator<List<RequestModel>>
    {
        private readonly IRequestData _requestRepo;

        public CreateRequestValidator(IRequestData requestRepo)
        {
            _requestRepo = requestRepo;
            RuleForEach(i => i).NotNull().NotEmpty().Must(i => !_requestRepo.ValidateCheckRequestIfExist(i.CompanyIdFrom, i.CompanyIdTo).Result).WithMessage("Request already exists.");
        }
    }

   
}
