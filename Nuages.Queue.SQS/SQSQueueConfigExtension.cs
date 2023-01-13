using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable MemberCanBePrivate.Global

namespace Nuages.Queue.SQS;

// ReSharper disable once InconsistentNaming
[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public static class SQSQueueConfigExtension
{
    
    // ReSharper disable once InconsistentNaming
    public static IServiceCollection AddSQSQueue(this IServiceCollection services)
    {
        services.AddScoped<ISQSQueueService, SQSQueueService>()
                .AddScoped<ISQSQueueClientProvider, SQSQueueClientProvider>();
        
        return services;
    }
}