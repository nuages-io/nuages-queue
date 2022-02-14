using Microsoft.Extensions.Options;
using Nuages.Queue.SQS;

namespace Nuages.Queue.Samples.SQS.Web;

public class SampleWorker : QueueWorker<ISQSQueueService>
{
    private readonly ILogger<QueueWorker<ISQSQueueService>> _logger;

    public SampleWorker(IServiceProvider serviceProvider, ILogger<SampleWorker> logger, IOptions<QueueWorkerOptions> options) 
        : base(serviceProvider, logger, options)
    {
        _logger = logger;
    }

    protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        await Task.Run(() =>
        {
            _logger.LogInformation("Message : {Message}", msg.Body);
            
            Console.WriteLine(msg.Body);

        });
       
        return true;
    }
}