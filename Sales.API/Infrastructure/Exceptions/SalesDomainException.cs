using System;
namespace Sales.API.Infrastructure.Exceptions
{
    public class SalesDomainException : Exception
    {
        public SalesDomainException()
        { }

        public SalesDomainException(string message)
            : base(message)
        { }

        public SalesDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

