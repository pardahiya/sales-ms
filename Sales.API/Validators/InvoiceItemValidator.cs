using FluentValidation;
using System;

namespace Sales.API.Validators
{
    public class InvoiceItemValidator : AbstractValidator<InvoiceItem>
    {
        public InvoiceItemValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.ProductCode).NotEmpty();
            RuleFor(x => x.UnitPrice).GreaterThan(0);

            // other relevant validations...
        }
    }
}
