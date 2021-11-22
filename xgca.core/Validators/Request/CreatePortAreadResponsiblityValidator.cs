using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using xas.data.DataModel.PortArea;
using xgca.core.Models.Accreditation.PortArea;

namespace xgca.core.Validators.Request
{
    public class CreatePortAreadResponsiblityValidator : AbstractValidator<List<CreatePortAreaModel>>
    {
        private readonly IPortAreaData _portAreaData;
        public CreatePortAreadResponsiblityValidator(IPortAreaData portAreaData)
        {
            _portAreaData = portAreaData;
            RuleForEach(i => i).NotNull().NotEmpty().Must(i => !_portAreaData.ValidateCheckPortAreaResponsibilityIfExist(i.RequestId, i.PortId).Result).WithMessage("Port area of responsibility already exists.");
        }
    }
}
