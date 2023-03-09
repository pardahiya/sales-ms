using System.Text.Json.Serialization;

namespace Sales.API.IntegrationEvents.Events;

public record InvoiceItemCreatedIntegrationEvent : IntegrationEvent
{
    [JsonInclude]
    public InvoiceItem NewInvoiceItem { get; set; }
    
    [JsonConstructor]
    public InvoiceItemCreatedIntegrationEvent(InvoiceItem invoiceItem)
    {
        NewInvoiceItem = invoiceItem;
    }
}
