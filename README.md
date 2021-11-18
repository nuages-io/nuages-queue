# Nuages.Queue

Nuages.Queue introduce the QueryWorker abstract class which is responsible to get message from a queue. The class needs to inherited from in order to provide overload for queue manipulations.

Two pre-build package are available on nuget :

- [Nuages.Queue.SQS](https://www.nuget.org/packages/Nuages.Queue.SQS/) for Simple Queue Service (SQS) on AWS
- [Nuages.Queue.ASQ](https://www.nuget.org/packages/Nuages.Queue.ASQ/) for Azure Storage Queue on Azure


##Using Nuages.Queue.SQS

See [Nuages.Queue.Samples.SQS.Console](https://github.com/nuages-io/nuages-queue/tree/main/Nuages.Queue.Samples.SQS.Console) for a full working sample.

Follow those steps to use Nuages.Queue.SQS in your .NET 6 console project. 

1. Create the project

```cmd
dotnet new console -n Nuages.Queue.Samples.SQS.Console
cd Nuages.Queue.Samples.SQS.Console
```

2. Add a package reference to Nuages.Queue.SQS

```cmd
dotnet add package Nuages.Queue.SQS 
```

3. Change the Project Sdk to Microsoft.NET.Sdk.Worker

```xml
<Project Sdk="Microsoft.NET.Sdk.Worker">
```

5. Add other references to the .csproj

```xml
<ItemGroup>
  <PackageReference Include="AWSSDK.Extensions.NETCore.Setup" Version="3.7.1" />
  <PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Hosting" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Logging" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Logging.Console" Version="6.0.0" />
</ItemGroup>
```


6. Optional. Add a new appsettings.json configuration file and set SQS credential info. 

```json

{
  "Queues":
  {
    "AutoCreateQueue" : true
  },
  "QueueWorker" :
  {
    "QueueName" : "queue-name-goes-here",
    "Enabled" : true,
    "MaxMessagesCount" : 1,
    "WaitDelayInMillisecondsWhenNoMessages": 2000
  }
}
```


7. Create your worker class. The following sample worker output message to the console.

```csharp 
using Microsoft.Extensions.Options;
using Nuages.Queue.SQS;

namespace Nuages.Queue.Samples.SQS.Console;

// ReSharper disable once ClassNeverInstantiated.Global
public class SampleWorker : QueueWorker<ISQSQueueService>
{
    private readonly ILogger<QueueWorker<ISQSQueueService>> _logger;

    public SampleWorker(IServiceProvider serviceProvider, ILogger<SampleWorker> logger, IOptions<QueueWorkerOptions> options) : base(serviceProvider, logger, options)
    {
        _logger = logger;
    }

    protected override async Task<bool> ProcessMessageAsync(QueueMessage msg)
    {
        await Task.Run(() =>
        {
            _logger.LogInformation("Message : {Message}", msg.Body);
            
            System.Console.WriteLine(msg.Body);

        });
       
        return true;
    }
} 
```

8. Finally, add the following code to your Program.cs file

```csharp

using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using Nuages.Queue;
using Nuages.Queue.Samples.SQS.Console;
using Nuages.Queue.SQS;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

var hostBuilder = new HostBuilder()
    .ConfigureLogging(logging => { logging.AddConsole(); })
    .ConfigureServices(services =>
        {
            services
                .AddSingleton(configuration)
                .AddQueueWorker<SampleWorker>(configuration);

            AddSQS(services);
        }
    );

var host = hostBuilder.UseConsoleLifetime().Build();

await SendTestMessageAsync(host.Services);

await host.RunAsync();

async Task SendTestMessageAsync(IServiceProvider provider)
{
    var queueService = provider.GetRequiredService<ISQSQueueService>();
    var options = provider.GetRequiredService<IOptions<QueueWorkerOptions>>().Value;

    var  fullName = await queueService.GetQueueFullNameAsync(options.QueueName);
    await queueService.EnqueueMessageAsync(fullName!, "Started!!!");
}

// ReSharper disable once InconsistentNaming
void AddSQS(IServiceCollection services, bool useProfile = true)
{
    if (useProfile)
    {
        //By default, we use a SQS profile to get credentials https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-netcore.html
        services.AddDefaultAWSOptions(configuration.GetAWSOptions())
            .AddAWSService<IAmazonSQS>();
    }
    else
    {
        var section = configuration.GetSection("SQS");
        var accessKey = section["AccessKey"];
        var secretKey = section["SecretKey"];
        var region = section["Region"];

        var sqsClient = new AmazonSQSClient(new BasicAWSCredentials(accessKey, secretKey),
            RegionEndpoint.GetBySystemName(region));

        services.AddSingleton<IAmazonSQS>(sqsClient);
    }
}

```


##Using Nuages.Queue.ASQ

The code is similar to the SQS sample.

See [Nuages.Queue.Samples.ASQ.Console](https://github.com/nuages-io/nuages-queue/tree/main/Nuages.Queue.Samples.ASQ.Console) for a full working sample.