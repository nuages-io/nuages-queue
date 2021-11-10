using System.Diagnostics.CodeAnalysis;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Nuages.Queue.SQS;

// ReSharper disable once InconsistentNaming
public class SQSQueueService : ISQSQueueService
{
    private readonly IQueueClientProvider _sqsProvider;
    private readonly QueueOptions _queryOptions;

    public SQSQueueService(IQueueClientProvider sqsProvider, IOptions<QueueOptions> queryOptions)
    {
        _sqsProvider = sqsProvider;
        _queryOptions = queryOptions.Value;
    }

    public async Task<string?> EnqueueMessageAsync(string queueFullName, string text)
    {
        var request = new SendMessageRequest
        {
            QueueUrl = queueFullName,
            MessageBody = text
        };
        
        var result = await _sqsProvider.GetClient(GetShortName(queueFullName)).SendMessageAsync(request);

        return result.MessageId;
    }

    private static string GetShortName(string queueFullName)
    {
        return queueFullName.Split('/').Last();
    }

    public async Task<List<QueueMessage>> DequeueMessageAsync(string queueFullName, int maxMessages = 1)
    {
        var list = new List<QueueMessage>();

        var request = new ReceiveMessageRequest
        {
            QueueUrl = queueFullName,
            MaxNumberOfMessages = maxMessages
        };

        var messages = await _sqsProvider.GetClient(queueFullName).ReceiveMessageAsync(request);
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var message in messages.Messages)
            list.Add(new QueueMessage
            {
                MessageId = message.MessageId,
                Body = message.Body,
                Handle = message.ReceiptHandle
            });

        return list;
    }

    public async Task DeleteMessageAsync(string queueFullName, string id, string receiptHandle)
    {
        var request = new DeleteMessageRequest
        {
            QueueUrl = queueFullName,
            ReceiptHandle = receiptHandle
        };

        await _sqsProvider.GetClient(GetShortName(queueFullName)).DeleteMessageAsync(request);
    }

    [ExcludeFromCodeCoverage] //Not able to test, Mock does not work
    public async Task<string?> GetQueueFullNameAsync(string queueName)
    {
        try
        {
            var response = await _sqsProvider.GetClient(queueName).GetQueueUrlAsync(new GetQueueUrlRequest
            {
                QueueName = queueName
            });

            return response?.QueueUrl;
        }
        catch (QueueDoesNotExistException)
        {
            if (_queryOptions.AutoCreateQueue)
            {
                //You might want to add additionale exception handling here because that may fail
                var response = await _sqsProvider.GetClient(queueName).CreateQueueAsync(new CreateQueueRequest
                {
                    QueueName = queueName
                });

                return response.QueueUrl;
            }

            throw;
        }
    }
}