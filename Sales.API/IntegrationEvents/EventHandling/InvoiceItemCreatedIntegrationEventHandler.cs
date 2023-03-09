using Sales.API.IntegrationEvents;
using Sales.API.IntegrationEvents.Events;

namespace Sales.API.IntegrationEvents.EventHandling;
    
public class InvoiceItemCreatedIntegrationEventHandler :
    IIntegrationEventHandler<InvoiceItemCreatedIntegrationEvent>
{
    private readonly SalesContext _salesContext;
    private readonly ISalesIntegrationEventService _salesIntegrationEventService;
    private readonly ILogger<InvoiceItemCreatedIntegrationEventHandler> _logger;

    public InvoiceItemCreatedIntegrationEventHandler(
        SalesContext salesContext,
        ISalesIntegrationEventService salesIntegrationEventService,
        ILogger<InvoiceItemCreatedIntegrationEventHandler> logger)
    {
        _salesContext = salesContext;
        _salesIntegrationEventService = salesIntegrationEventService;
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
    }

    public async Task Handle(InvoiceItemCreatedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

            //TODO: implement any relevant business logic

            // if work done here requires additional event, raise it here

            _logger.LogInformation("----- Handled integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
        }
    }
}
