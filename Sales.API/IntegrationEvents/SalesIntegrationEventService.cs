using Sales.IntegrationEventLogEF.Services;
using Sales.IntegrationEventLogEF.Utilities;

namespace Sales.API.IntegrationEvents;

public class SalesIntegrationEventService : ISalesIntegrationEventService, IDisposable
{
    private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
    private readonly IEventBus _eventBus;
    private readonly SalesContext _salesContext;
    private readonly IIntegrationEventLogService _eventLogService;
    private readonly ILogger<SalesIntegrationEventService> _logger;
    private volatile bool disposedValue;

    public SalesIntegrationEventService(
        ILogger<SalesIntegrationEventService> logger,
        IEventBus eventBus,
        SalesContext salesContext,
        IIntegrationEventLogService eventLogService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _salesContext = salesContext ?? throw new ArgumentNullException(nameof(salesContext));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _eventLogService = eventLogService ?? throw new ArgumentNullException(nameof(eventLogService));
    }

    public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
    {
        try
        {
            _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

            await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
            _eventBus.Publish(evt);
            await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
            await _eventLogService.MarkEventAsFailedAsync(evt.Id);
        }
    }

    public async Task SaveEventAndSalesContextChangesAsync(IntegrationEvent evt)
    {
        _logger.LogInformation("----- SalesIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

        await ResilientTransaction.New(_salesContext).ExecuteAsync(async () =>
        {
            // for atomicity between sales database and IntegrationEventLog
            await _salesContext.SaveChangesAsync();
            await _eventLogService.SaveEventAsync(evt, _salesContext.Database.CurrentTransaction);
        });
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                (_eventLogService as IDisposable)?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
