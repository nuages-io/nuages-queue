using Amazon.SQS;

namespace Nuages.Queue.SQS;

// ReSharper disable once InconsistentNaming
public class SQSQueueClientProvider : ISQSQueueClientProvider
{
    private readonly IAmazonSQS _sqs;

    public SQSQueueClientProvider(IAmazonSQS sqs)
    {
        _sqs = sqs;
    }
    
    public IAmazonSQS GetClient(string queueName)
    {
        return _sqs;
    }
}