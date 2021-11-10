using Amazon.SQS;

namespace Nuages.Queue.SQS;

public class QueueClientProvider : IQueueClientProvider
{
    private readonly IAmazonSQS _sqs;

    public QueueClientProvider(IAmazonSQS sqs)
    {
        _sqs = sqs;
    }
    
    public IAmazonSQS GetClient(string queueName)
    {
        return _sqs;
    }
}