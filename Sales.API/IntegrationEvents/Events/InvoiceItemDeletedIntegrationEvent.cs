namespace Sales.API.IntegrationEvents.Events;

public record InvoiceItemDeletedIntegrationEvent : IntegrationEvent
{
    public InvoiceItem DeletedItem { get; }

    public InvoiceItemDeletedIntegrationEvent(InvoiceItem invoiceItem)
    {
        DeletedItem = invoiceItem;
    }
}