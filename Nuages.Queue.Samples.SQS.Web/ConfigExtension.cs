namespace Nuages.Queue.Samples.SQS.Web;

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
        IConfiguration configuration, string name,
        Action<QueueOptions>? configureQueues = null,
        Action<QueueWorkerOptions>? configureWorker = null)
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
        
    }
    
}