namespace Sales.API.IntegrationEvents.Events;

public record InvoiceUpdatedIntegrationEvent : IntegrationEvent
{
    public Invoice OldInvoice { get; private init; }
    public Invoice NewInvoice { get; private init; }

    public InvoiceUpdatedIntegrationEvent(Invoice oldInvoice, Invoice newInvoice)
    {
        OldInvoice = oldInvoice;
        NewInvoice = newInvoice;
    }
}
