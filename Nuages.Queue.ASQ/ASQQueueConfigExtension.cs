using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable MemberCanBePrivate.Global

namespace Nuages.Queue.ASQ;

[ExcludeFromCodeCoverage]
// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedMember.Global
// ReSharper disable once UnusedType.Global
public static class ASQQueueConfigExtension
{

    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMember.Global
    public static IServiceCollection AddASQQueue(this IServiceCollection services)
    {
        services.AddScoped<IASQQueueService, ASQQueueService>()
            .AddScoped<IASQQueueClientProvider, ASQQueueClientProvider>();
        
        return services;
    }
}