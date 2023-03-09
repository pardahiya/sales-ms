using FluentValidation;
using System;

namespace Sales.API.Validators
{
    public class InvoiceValidator : AbstractValidator<Invoice>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.InvoiceNumber).GreaterThan(0);

            // other relevant validations...
        }
    }
}
