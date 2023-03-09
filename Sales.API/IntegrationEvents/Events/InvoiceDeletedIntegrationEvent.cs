namespace Sales.API.IntegrationEvents.Events;

public record InvoiceDeletedIntegrationEvent : IntegrationEvent
{
    public Invoice DeletedInvoice { get; }
    public InvoiceDeletedIntegrationEvent(Invoice invoice)
    {
        DeletedInvoice = invoice;
    }
}
