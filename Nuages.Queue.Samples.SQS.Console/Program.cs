using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using Nuages.Queue;
using Nuages.Queue.Samples.SQS.Console;
using Nuages.Queue.SQS;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName!)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

const string workerName = "SampleWorker";

var hostBuilder = new HostBuilder()
    .ConfigureLogging(logging => { logging.AddConsole(); })
    .ConfigureServices(services =>
        {
            services
                .AddSingleton(configuration)
                .AddSQSQueue()
                .Configure<QueueWorkerOptions>(workerName, configuration.GetSection("QueueWorker"))
                .Configure<QueueOptions>(workerName, configuration.GetSection("Queues"))
                .AddSingleton<IHostedService>(x =>
                    ActivatorUtilities.CreateInstance<SampleWorker>(x, workerName));
                
            AddSQS(services);
        }
    );

var host = hostBuilder.UseConsoleLifetime().Build();

await SendTestMessageAsync(host.Services);

await host.RunAsync();

async Task SendTestMessageAsync(IServiceProvider provider)
{
    var queueService = provider.GetRequiredService<ISQSQueueService>();
    var options = provider.GetRequiredService<IOptionsMonitor<QueueWorkerOptions>>().Get("SampleWorker");

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