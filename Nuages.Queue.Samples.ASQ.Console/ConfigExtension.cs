using System.Diagnostics.CodeAnalysis;
using Nuages.Queue.ASQ;


// ReSharper disable once InconsistentNaming
namespace Nuages.Queue.Samples.ASQ.Console;

[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
public static class ConfigExtension
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    // ReSharper disable once UnusedMember.Global
    public static IServiceCollection AddSampleWorker(this IServiceCollection services, 
        IConfiguration configuration, string name,
        Action<QueueOptions> configureQueues = null,
        Action<QueueWorkerOptions> configureWorker = null)
    {
        services.Configure<QueueWorkerOptions>(name, configuration.GetSection("QueueWorker"));
        services.Configure<QueueOptions>(name, configuration.GetSection("Queues"));

        if (configureWorker != null)
            services.Configure(name, configureWorker);
        
        if (configureQueues != null)
            services.Configure(name, configureQueues);

        return services.AddSingleton<IHostedService>(x =>
            ActivatorUtilities.CreateInstance<SampleWorker>(x, name)
        );
        
        // return services.AddScoped<ISQSQueueService, SQSQueueService>()
        //     .AddScoped<ISQSQueueClientProvider, SQSQueueClientProvider>()
        //     .AddHostedService<SampleWorker>();
    }
    
}