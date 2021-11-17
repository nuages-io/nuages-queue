namespace Nuages.Queue.Samples.SQS;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nuages.Queue.SQS;


// ReSharper disable once InconsistentNaming
[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public static class ConfigExtension
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    // ReSharper disable once UnusedMember.Global
    public static IServiceCollection AddSampleWorker(this IServiceCollection services, 
        IConfiguration configuration,
        Action<QueueOptions>? configureQueues = null,
        Action<QueueWorkerOptions>? configureWorker = null)
    {
        services.Configure<QueueWorkerOptions>(configuration.GetSection("QueueWorker"));
        services.Configure<QueueOptions>(configuration.GetSection("Queues"));

        if (configureWorker != null)
            services.Configure(configureWorker);
        
        if (configureQueues != null)
            services.Configure(configureQueues);

        return services.AddScoped<ISQSQueueService, SQSQueueService>()
            .AddScoped<IQueueClientProvider, QueueClientProvider>()
            .AddHostedService<SampleWorker>();
    }
    
}