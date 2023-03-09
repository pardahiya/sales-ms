using System;
namespace Sales.API.Model
{
	public class InvoiceItem
	{
        public Guid Id { get; set; }
        public long Quantity { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
    }
}

