using Azure.Storage.Queues;

namespace Nuages.Queue.ASQ;

// ReSharper disable once InconsistentNaming
public interface IASQQueueClientProvider
{
    QueueClient GetClient(string queueName);
}