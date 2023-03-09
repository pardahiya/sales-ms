using FluentValidation;
using Sales.API.IntegrationEvents;
using Sales.API.IntegrationEvents.Events;

// This is a template for InvoiceItem endpoints, similar endpoints can be added for other resources
namespace Sales.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SalesController: ControllerBase
	{
        private readonly SalesContext _salesContext;
        private readonly SalesSettings _settings;
        private readonly ISalesIntegrationEventService _salesIntegrationEventService;
        private readonly IValidator<InvoiceItem> _invoiceItemValidator;

        public SalesController(
            SalesContext context, 
            IOptionsSnapshot<SalesSettings> settings, 
            ISalesIntegrationEventService salesIntegrationEventService,
            IValidator<InvoiceItem> invoiceItemValidator)
        {
            _salesContext = context ?? throw new ArgumentNullException(nameof(context));
            _salesIntegrationEventService = salesIntegrationEventService ?? throw new ArgumentNullException(nameof(salesIntegrationEventService));
            _settings = settings.Value;
            _invoiceItemValidator = invoiceItemValidator;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route("items/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(InvoiceItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InvoiceItem>> ItemByIdAsync(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return BadRequest();
            }

            var item = await _salesContext.InvoiceItems.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        //PUT api/v1/[controller]/items
        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateProductAsync([FromBody] InvoiceItem itemToUpdate)
        {
            var invoiceItem = await _salesContext.InvoiceItems.SingleOrDefaultAsync(i => i.Id == itemToUpdate.Id);

            if (invoiceItem == null)
            {
                return NotFound(new { Message = $"Item with id {itemToUpdate.Id} not found." });
            }
            
            var result = await _invoiceItemValidator.ValidateAsync(invoiceItem);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var invoiceItemUpdatedEvent = new InvoiceItemUpdatedIntegrationEvent(invoiceItem, itemToUpdate);

            // Update current product
            invoiceItem = itemToUpdate;
            _salesContext.InvoiceItems.Update(invoiceItem);

            await _salesIntegrationEventService.SaveEventAndSalesContextChangesAsync(invoiceItemUpdatedEvent);
            await _salesIntegrationEventService.PublishThroughEventBusAsync(invoiceItemUpdatedEvent);

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = itemToUpdate.Id }, null);
        }

        //POST api/v1/[controller]/items
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateInvoiceItemAsync([FromBody] InvoiceItem invoiceItem)
        {
            var result = await _invoiceItemValidator.ValidateAsync(invoiceItem);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(x => x.ErrorMessage).ToList());
            }

            invoiceItem.Id = Guid.NewGuid();

            _salesContext.InvoiceItems.Add(invoiceItem);
            await _salesContext.SaveChangesAsync();

            var invoiceItemUpdatedEvent = new InvoiceItemCreatedIntegrationEvent(invoiceItem);

            await _salesIntegrationEventService.SaveEventAndSalesContextChangesAsync(invoiceItemUpdatedEvent);
            await _salesIntegrationEventService.PublishThroughEventBusAsync(invoiceItemUpdatedEvent);

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = invoiceItem.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteInvoiceItemAsync(Guid id)
        {
            var invoiceItem = _salesContext.InvoiceItems.SingleOrDefault(x => x.Id == id);

            if (invoiceItem == null)
            {
                return NotFound();
            }

            _salesContext.InvoiceItems.Remove(invoiceItem);

            await _salesContext.SaveChangesAsync();

            var invoiceItemUpdatedEvent = new InvoiceItemDeletedIntegrationEvent(invoiceItem);

            await _salesIntegrationEventService.SaveEventAndSalesContextChangesAsync(invoiceItemUpdatedEvent);
            await _salesIntegrationEventService.PublishThroughEventBusAsync(invoiceItemUpdatedEvent);

            return NoContent();
        }
    }
}

