using System;
namespace Sales.API.Infrastructure
{
    public class SalesContextSeed
    {
        public async Task SeedAsync(SalesContext context, IWebHostEnvironment env, IOptions<SalesSettings> settings, ILogger<SalesContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(SalesContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                //var useCustomizationData = settings.Value.UseCustomizationData;
                var contentRootPath = env.ContentRootPath;
                var picturePath = env.WebRootPath;

                if (!context.InvoiceItems.Any())
                {
                    await context.InvoiceItems.AddAsync(new InvoiceItem()
                    {
                        Id = Guid.NewGuid(),
                        Amount = 10,
                        ProductCode = "t",
                        ProductName = "t",
                        Quantity =1,
                        UnitPrice = 6
                    });

                    await context.SaveChangesAsync();
                }
            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<SalesContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}

