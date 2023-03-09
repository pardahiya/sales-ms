using Sales.API.IntegrationEvents.Events;

namespace Sales.API.IntegrationEvents.EventHandling;

public class InvoiceCreatedIntegrationEventHandler :
    IIntegrationEventHandler<InvoiceCreatedIntegrationEvent>
{
    private readonly SalesContext _salesContext;
    private readonly ILogger<InvoiceCreatedIntegrationEventHandler> _logger;

    public InvoiceCreatedIntegrationEventHandler(
        SalesContext salesContext,
        ILogger<InvoiceCreatedIntegrationEventHandler> logger)
    {
        _salesContext = salesContext;
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
    }

    public async Task Handle(InvoiceCreatedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

            //TODO: implement any business logic here like sending an email

            // if work done here requires additional event, raise it here

            _logger.LogInformation("----- Handled integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
        }
    }
}
