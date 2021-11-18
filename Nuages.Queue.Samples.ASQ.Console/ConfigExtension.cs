namespace Nuages.Queue.Samples.ASQ.Console;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nuages.Queue.ASQ;


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
        services.Configure<ASQQueueClientOptions>(configuration.GetSection("ASQ"));
        services.Configure<QueueOptions>(configuration.GetSection("Queues"));

        if (configureWorker != null)
            services.Configure(configureWorker);
        
        if (configureQueues != null)
            services.Configure(configureQueues);

        return services.AddScoped<IASQQueueService, ASQQueueService>()
            .AddScoped<IQueueClientProvider, QueueClientProvider>()
            .AddHostedService<SampleWorker>();
    }
    
}