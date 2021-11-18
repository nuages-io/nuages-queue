using System.ComponentModel.DataAnnotations;

using Nuages.Queue.Samples.Simple.Console.SimpleQueue.Queue;


namespace Nuages.Queue.Samples.Simple.Console.SimpleQueue;

// ReSharper disable once InconsistentNaming
public static class SimpleTaskConfig
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddSimpleTaskQueueWorker(this IServiceCollection services, 
        IConfiguration configuration,
        // ReSharper disable once InconsistentNaming
        Action<QueueWorkerOptions>? configureWorker = null)
    {
        services.Configure<QueueWorkerOptions>(configuration.GetSection("TaskQueueWorker"));
        
        if (configureWorker != null)
            services.Configure(configureWorker);
        
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

        return services.AddSingleton<ISimpleQueueService, SimpleQueueService>()
            .AddHostedService<SimpleQueueWorker>();
    }

    private static IEnumerable<string> ValidationErrors(object option)
    {
        var context = new ValidationContext(option, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(option, context, results, true);
        foreach (var validationResult in results) yield return validationResult.ErrorMessage ?? "?";
    }
}