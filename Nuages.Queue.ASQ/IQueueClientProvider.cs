using Azure.Storage.Queues;

namespace Nuages.Queue.ASQ;

public interface IQueueClientProvider
{
    QueueClient GetClient(string queueName);
}