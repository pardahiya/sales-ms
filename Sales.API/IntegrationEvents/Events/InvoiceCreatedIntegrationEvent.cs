namespace Sales.API.IntegrationEvents.Events;

public record InvoiceCreatedIntegrationEvent : IntegrationEvent
{
    public Invoice NewInvoice { get; }
    public InvoiceCreatedIntegrationEvent(Invoice invoice)
    {
        NewInvoice = invoice;
    }
}
