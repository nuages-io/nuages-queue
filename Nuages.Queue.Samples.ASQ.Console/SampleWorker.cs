using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nuages.Queue.ASQ;

namespace Nuages.Queue.Samples.ASQ.Console;

public class SampleWorker : QueueWorker<IASQQueueService>
{
    private readonly ILogger<QueueWorker<IASQQueueService>> _logger;

    public SampleWorker(IServiceProvider serviceProvider, ILogger<QueueWorker<IASQQueueService>> logger, IOptions<QueueWorkerOptions> options) : base(serviceProvider, logger, options)
    {
        _logger = logger;
    }

    protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        await Task.Run(() =>
        {
            System.Console.WriteLine(msg.Body);

        });
       
        return true;
    }
}