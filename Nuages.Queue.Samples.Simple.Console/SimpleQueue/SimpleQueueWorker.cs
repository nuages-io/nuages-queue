using Microsoft.Extensions.Options;

using Nuages.Queue.Samples.Simple.Console.SimpleQueue.Queue;

namespace Nuages.Queue.Samples.Simple.Console.SimpleQueue;

public class SimpleQueueWorker : QueueWorker<ISimpleQueueService>
{
    public SimpleQueueWorker(IServiceProvider serviceProvider, ILogger<SimpleQueueWorker> logger, IOptions<QueueWorkerOptions> queueWorkerOptions) : 
        base(serviceProvider, logger, queueWorkerOptions)
    {
       
    }

    protected override async Task<List<QueueMessage>> ReceiveMessageAsync(ISimpleQueueService queueService)
    {
        return await queueService.DequeueMessageAsync(QueueName!, MaxMessagesCount);
    }

    protected override async Task DeleteMessageAsync(ISimpleQueueService queueService, string id, string receiptHandle)
    {
        await queueService.DeleteMessageAsync(QueueName!, id, receiptHandle);
    }

    protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        Logger.LogInformation("Message : {Message}", msg.Body);
        
        return await Task.FromResult(true);
    }
}