using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace Nuages.Queue.ASQ;

[ExcludeFromCodeCoverage]
// ReSharper disable once InconsistentNaming
public class ASQQueueService : IASQQueueService
{
    private readonly IQueueClientProvider _clientProvider;
    private readonly QueueOptions _queryOptions;

    public ASQQueueService(IQueueClientProvider clientProvider, IOptions<QueueOptions> queryOptions)
    {
        _clientProvider = clientProvider;
        _queryOptions = queryOptions.Value;
    }
    
    public async Task<string?> GetQueueFullNameAsync(string queueName)
    {
        return await Task.FromResult(queueName);
    }

    public async Task<string?> EnqueueMessageAsync(string fullQueueName, string text)
    {
        var client = _clientProvider.GetClient(fullQueueName);

        // Create the queue if it doesn't already exist
        if (_queryOptions.AutoCreateQueue)
            await client.CreateIfNotExistsAsync();

        var res = await client.SendMessageAsync(text);
        
        return res.Value.MessageId;
    }

    public async Task<List<QueueMessage>> DequeueMessageAsync(string fullQueueName, int maxMessages = 1)
    {
        var client = _clientProvider.GetClient(fullQueueName);
        
        if (_queryOptions.AutoCreateQueue)
            await client.CreateIfNotExistsAsync();
        
        var messages = await client.ReceiveMessagesAsync(maxMessages);

        return messages.Value.Select(message => new QueueMessage { MessageId = message.MessageId, Body = message.MessageText, Handle = message.PopReceipt }).ToList();
    }

    public async Task DeleteMessageAsync(string fullQueueName, string id, string receiptHandle)
    {
        var client = _clientProvider.GetClient(fullQueueName);
        
        if (_queryOptions.AutoCreateQueue)
            await client.CreateIfNotExistsAsync();
        
        await client.DeleteMessageAsync(id, receiptHandle);
    }
}