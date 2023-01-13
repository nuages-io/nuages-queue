using Nuages.Queue.Samples.Simple.Console.SimpleQueue.Queue;


namespace Nuages.Queue.Samples.Simple.Console.SimpleQueue;

// ReSharper disable once InconsistentNaming
public static class SimpleTaskConfig
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddSimpleTaskQueueWorker(this IServiceCollection services, 
        IConfiguration configuration, string name,
        // ReSharper disable once InconsistentNaming
        Action<QueueWorkerOptions>? configureWorker = null)
    {
        services.Configure<QueueWorkerOptions>(name, configuration.GetSection("QueueWorker"));
        
        if (configureWorker != null)
            services.Configure(configureWorker);

        return services.AddSingleton<ISimpleQueueService, SimpleQueueService>()
            .AddSingleton<IHostedService>(x =>
                ActivatorUtilities.CreateInstance<SimpleQueueWorker>(x, name));
    }

   
}