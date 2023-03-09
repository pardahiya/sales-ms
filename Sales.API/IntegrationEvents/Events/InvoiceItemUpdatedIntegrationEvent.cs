namespace Sales.API.IntegrationEvents.Events;

public record InvoiceItemUpdatedIntegrationEvent : IntegrationEvent
{
    public InvoiceItem OldInvoiceItem { get; private init; }
    public InvoiceItem NewInvoiceItem { get; private init; }

    public InvoiceItemUpdatedIntegrationEvent(InvoiceItem oldInvoiceItem, InvoiceItem newInvoiceItem)
    {
        OldInvoiceItem = oldInvoiceItem;
        NewInvoiceItem = newInvoiceItem;
    }
}
