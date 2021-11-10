using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Nuages.Queue;

    [ExcludeFromCodeCoverage]
  
    // ReSharper disable once UnusedType.Global
    public abstract class QueueWorker<T> : BackgroundService where T : IQueueService
    {
        protected string? QueueName { get; set; }
        protected string? QueueNameFullName { get; set; }
        
        protected int MaxMessagesCount { get; set; } = 10;
        protected int WaitDelayInMillisecondsWhenNoMessages { get; set; } = 1000;
        
        protected readonly IServiceProvider ServiceProvider;
        protected readonly ILogger<QueueWorker<T>> Logger;
        
        protected readonly QueueWorkerOptions Options;
        
        protected QueueWorker(IServiceProvider serviceProvider, ILogger<QueueWorker<T>> logger, 
            IOptions<QueueWorkerOptions> options)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
            
            Options = options.Value;
           
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var enable = Options.Enabled;
            if (!enable)
                return;
             
            QueueName = Options.QueueName;
            MaxMessagesCount = Options.MaxMessagesCount;
            WaitDelayInMillisecondsWhenNoMessages = Options.WaitDelayInMillisecondsWhenNoMessages;
            
            if (string.IsNullOrEmpty(QueueName))
                throw new NullReferenceException("QueueName must be provided");
            
            using var scope = ServiceProvider.CreateScope();

            var queueService = scope.ServiceProvider.GetRequiredService<T>();

            QueueNameFullName = await queueService.GetQueueFullNameAsync(QueueName!);
            if (string.IsNullOrEmpty(QueueNameFullName))
                throw new Exception($"Queue Url not found for {QueueName}");
            
            InitializeDependencies(scope);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var messages = await ReceiveMessageAsync(queueService);

                    if (messages.Any())
                    {
                        LogInformation($"{messages.Count} messages received");

                        foreach (var msg in messages)
                        {
                            var result = await ProcessMessageAsync(msg);

                            if (result)
                            {
                                LogInformation($"{msg.MessageId} processed with success ");
                                await DeleteMessageAsync(queueService, msg.MessageId, msg.Handle);
                            }
                        }
                    }
                    else
                    {
                        LogInformation("0 messages received");
                        await Task.Delay(TimeSpan.FromMilliseconds(WaitDelayInMillisecondsWhenNoMessages), stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
                
            }
        }

        protected virtual void InitializeDependencies(IServiceScope scope)
        {
            
        }

        protected virtual async Task<List<QueueMessage>> ReceiveMessageAsync(T queueService)
        {
            if (string.IsNullOrEmpty(QueueNameFullName))
                throw new NullReferenceException(QueueNameFullName);
        
            return await queueService.DequeueMessageAsync(QueueNameFullName, MaxMessagesCount);
        }

        protected virtual async Task DeleteMessageAsync(T queueService, string id, string receiptHandle)
        {
            if (string.IsNullOrEmpty(QueueNameFullName))
                throw new NullReferenceException(QueueNameFullName);
        
            await queueService.DeleteMessageAsync(QueueNameFullName, id, receiptHandle);
        }

        protected virtual void LogInformation(string message)
        {
            Logger.LogInformation("{Message} : {QueueNameFullName}",message, QueueNameFullName);
        }
    
        protected virtual void LogError(string message)
        {
            Logger.LogError("{Message} : {QueueNameFullName}",message, QueueNameFullName);
        }
    
        protected abstract Task<bool> ProcessMessageAsync(QueueMessage msg);
    }