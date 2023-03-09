using System;
namespace Sales.API.Model
{
	public class Invoice
	{
		public int Id { get; set; }
		public long InvoiceNumber { get; set; }
		public DateTime CreatedOn { get; set; }
		public double Amount { get; set; }
	}
}

