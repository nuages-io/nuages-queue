using System.Diagnostics.CodeAnalysis;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;

namespace Nuages.Queue.ASQ;

[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
// ReSharper disable once InconsistentNaming
public class ASQQueueClientProvider : IASQQueueClientProvider
{
    private readonly ASQQueueClientOptions _options;

    public ASQQueueClientProvider(IOptions<ASQQueueClientOptions> options)
    {
        _options = options.Value;
    }
    
    public QueueClient GetClient(string queueName)
    {
        return new QueueClient(_options.ConnectionString, queueName);
    }
}