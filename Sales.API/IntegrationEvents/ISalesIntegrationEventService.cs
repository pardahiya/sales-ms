using System.Threading.Tasks;
using Sales.EventBus.Events;

namespace Sales.API.IntegrationEvents;

public interface ISalesIntegrationEventService
{
    Task SaveEventAndSalesContextChangesAsync(IntegrationEvent evt);
    Task PublishThroughEventBusAsync(IntegrationEvent evt);
}