using System.Diagnostics.CodeAnalysis;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;

namespace Nuages.Queue.ASQ;

[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public class QueueClientProvider : IQueueClientProvider
{
    private readonly ASQQueueClientOptions _options;

    public QueueClientProvider(IOptions<ASQQueueClientOptions> options)
    {
        _options = options.Value;
    }
    
    public QueueClient GetClient(string queueName)
    {
        return new QueueClient(_options.ConnectionString, queueName);
    }
}