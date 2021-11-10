using Amazon.SQS;

namespace Nuages.Queue.SQS;

public interface IQueueClientProvider
{
    // ReSharper disable once UnusedParameter.Global
    IAmazonSQS GetClient(string queueName);
}