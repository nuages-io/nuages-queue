namespace Nuages.Queue.Samples.Simple.Console.SimpleQueue.Queue;

public class SimpleQueueService : ISimpleQueueService
{
    private readonly Queue<QueueMessage> _queue = new ();
    
    public async Task<string?> GetQueueFullNameAsync(string queueName)
    {
        return await Task.FromResult(queueName);
    }

    public async Task<string?> EnqueueMessageAsync(string queueFullName, string text)
    {
        var message = new QueueMessage
        {
            Body = text,
            MessageId = Guid.NewGuid().ToString()
        };
        
        _queue.Enqueue(message);

        return await Task.FromResult(message.MessageId);
    }

    public async Task<List<QueueMessage>> DequeueMessageAsync(string queueFullName, int maxMessages = 1)
    {
        var list = new List<QueueMessage>();
        var res = _queue.TryPeek(out var message);
        if (res && message != null)
        {
            message.Handle = Guid.NewGuid().ToString();
            list.Add(message);
        }

        return await Task.FromResult(list);
    }

    public async Task DeleteMessageAsync(string queueFullName, string id, string receiptHandle)
    {
        var res = _queue.TryPeek(out var message);
        if (res && message != null)
        {
            if (message.Handle == receiptHandle)
            {
                _queue.Dequeue();
            }
        }

        await Task.FromResult(0);
    }
}