using Amazon.SQS;

namespace Nuages.Queue.SQS;

// ReSharper disable once InconsistentNaming
public interface ISQSQueueClientProvider
{
    // ReSharper disable once UnusedParameter.Global
    IAmazonSQS GetClient(string queueName);
}