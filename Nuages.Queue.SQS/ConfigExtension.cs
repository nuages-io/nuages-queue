using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nuages.Queue.SQS;

// ReSharper disable once InconsistentNaming
[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public static class QueueSQSConfigExtension
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    // ReSharper disable once UnusedMember.Global
    public static IServiceCollection AddQueueWorker<T>(this IServiceCollection services, 
        IConfiguration configuration,
        Action<QueueOptions>? configureQueues = null,
        Action<QueueWorkerOptions>? configureWorker = null) where T : BackgroundService
    {
        services.Configure<QueueWorkerOptions>(configuration.GetSection("QueueWorker"));
        services.Configure<QueueOptions>(configuration.GetSection("Queues"));

        if (configureWorker != null)
            services.Configure(configureWorker);
        
        if (configureQueues != null)
            services.Configure(configureQueues);

        return services.AddScoped<ISQSQueueService, SQSQueueService>()
            .AddScoped<IQueueClientProvider, QueueClientProvider>()
            .AddHostedService<T>();
    }
}