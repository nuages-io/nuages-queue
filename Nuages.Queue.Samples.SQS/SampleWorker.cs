using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nuages.Queue.SQS;

namespace Nuages.Queue.Samples.SQS;

public class SampleWorker : QueueWorker<ISQSQueueService>
{
    private readonly ILogger<QueueWorker<ISQSQueueService>> _logger;

    public SampleWorker(IServiceProvider serviceProvider, ILogger<QueueWorker<ISQSQueueService>> logger, IOptions<QueueWorkerOptions> options) : base(serviceProvider, logger, options)
    {
        _logger = logger;
    }

    protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        await Task.Run(() =>
        {
            Console.WriteLine(msg.Body);

        });
       
        return true;
    }
}