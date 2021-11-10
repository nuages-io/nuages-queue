using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Nuages.Queue.SQS.Tests;

[ExcludeFromCodeCoverage]
// ReSharper disable once InconsistentNaming
public class SQSQueueServiceTests
{
     [Fact]
     public async Task PutMessageToQueue()
     {
         var queueName = Guid.NewGuid().ToString();
         
         var sqs = new Mock<IAmazonSQS>();
         sqs.Setup(s => s.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SendMessageResponse
         {
             MessageId = Guid.NewGuid().ToString()
         });

         var clientProvider = new QueueClientProvider(sqs.Object);

         IQueueService queueService = new SQSQueueService(clientProvider, Options.Create(new QueueOptions
         {
             AutoCreateQueue = true
         }));
         var res = await queueService.EnqueueMessageAsync(queueName,  JsonSerializer.Serialize("data"));
         
         Assert.NotNull(res);
     }
     
     // [Fact]
     // public async Task PutMessageToQueueUsingEnqueueTaskAsync()
     // {
     //     var queueName = Guid.NewGuid().ToString();
     //
     //     var sqs = new Mock<IAmazonSQS>();
     //     sqs.Setup(s => s.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SendMessageResponse
     //     {
     //         MessageId = Guid.NewGuid().ToString()
     //     });
     //     
     //     sqs.Setup(s => s.GetQueueUrlAsync(It.IsAny<GetQueueUrlRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetQueueUrlResponse
     //     {
     //         QueueUrl = queueName
     //     });
     //     
     //     var clientProvider = new QueueClientProvider(sqs.Object);
     //
     //     IQueueService queueService = new SQSQueueService(clientProvider, Options.Create(new QueueOptions()));
     //     
     //     var taskData = RunnableTaskCreator.Create(data);
     //     
     //     var res = await queueService.EnqueueTaskAsync<IQueueService, object>(queueName,  new {Test = "Message"});
     //     
     //     Assert.NotNull(res);
     // }

     [Fact]
     public async Task PollQueue()
     {
         var message = new Message
         {
             MessageId = Guid.NewGuid().ToString(),
             Body = "body",
             ReceiptHandle = Guid.NewGuid().ToString()
         };

         var clientProvider = new Mock<IQueueClientProvider>();
         
         var sqs = new Mock<IAmazonSQS>();

         clientProvider.Setup(c => c.GetClient("")).Returns(sqs.Object);
         
         sqs.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(new ReceiveMessageResponse
             {
                 Messages = new List<Message>
                 {
                     message
                 }
             });

         IQueueService queueService = new SQSQueueService(clientProvider.Object, Options.Create(new QueueOptions()));

         var res = await queueService.DequeueMessageAsync("");
         Assert.Single(res);
         Assert.Equal(message.Body, res.First().Body);
         Assert.Equal(message.ReceiptHandle, res.First().Handle);
         Assert.Equal(message.MessageId, res.First().MessageId);
     }

     [Fact]
     public async Task DeleteMessage()
     {
         var clientProvider = new Mock<IQueueClientProvider>();
         
         var sqs = new Mock<IAmazonSQS>();

         var queueName = Guid.NewGuid().ToString();
         
         clientProvider.Setup(c => c.GetClient(queueName)).Returns(sqs.Object);
         
         IQueueService queueService = new SQSQueueService(clientProvider.Object, Options.Create(new QueueOptions()));

         await queueService.DeleteMessageAsync(queueName,  "id", "handle");
     }
}