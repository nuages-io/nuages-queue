using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable MemberCanBePrivate.Global

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

        return AddQueueWorker<T>(services, configureQueues, configureWorker);
    }

    public static IServiceCollection AddQueueWorker<T>(this IServiceCollection services,
        Action<QueueOptions>? configureQueues = null,
        Action<QueueWorkerOptions>? configureWorker = null) where T : BackgroundService
    {
        if (configureWorker != null)
            services.Configure(configureWorker);
        
        if (configureQueues != null)
            services.Configure(configureQueues);

        services.PostConfigure<QueueWorkerOptions>(options =>
        {
            var configErrors = ValidationErrors(options).ToArray();
            // ReSharper disable once InvertIf
            if (configErrors.Any())
            {
                var aggregateErrors = string.Join(",", configErrors);
                var count = configErrors.Length;
                var configType = options.GetType().Name;
                throw new ApplicationException(
                    $"Found {count} configuration error(s) in {configType}: {aggregateErrors}");
            }
        });
        
        return services.AddScoped<ISQSQueueService, SQSQueueService>()
            .AddScoped<IQueueClientProvider, QueueClientProvider>()
            .AddHostedService<T>();
    }
    
    private static IEnumerable<string> ValidationErrors(object option)
    {
        var context = new ValidationContext(option, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(option, context, results, true);
        foreach (var validationResult in results) yield return validationResult.ErrorMessage ?? "?";
    }
}