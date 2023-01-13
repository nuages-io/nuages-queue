using Microsoft.Extensions.Options;
using Nuages.Queue.ASQ;

namespace Nuages.Queue.Samples.ASQ.Console;

// ReSharper disable once ClassNeverInstantiated.Global
public class SampleWorker : QueueWorker<IASQQueueService>
{
    private readonly ILogger<QueueWorker<IASQQueueService>> _logger;

    public SampleWorker(string name, IServiceProvider serviceProvider, ILogger<SampleWorker> logger, IOptionsMonitor<QueueWorkerOptions> options) : base(name, serviceProvider, logger, options)
    {
        _logger = logger;
    }

    protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        await Task.Run(() =>
        {
            _logger.LogInformation("Message : {Message}", msg.Body);
            System.Console.WriteLine(msg.Body);

        });
       
        return true;
    }
}