using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Nuages.Queue.SQS;
// ReSharper disable UnusedMember.Global

namespace Nuages.Queue.Samples.SQS.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ISQSQueueService _isqsQueueService;
    private readonly QueueWorkerOptions _options1;
    private readonly QueueWorkerOptions _options2;
    
    public IndexModel( ISQSQueueService isqsQueueService, IOptionsMonitor<QueueWorkerOptions> options)
    {
        _isqsQueueService = isqsQueueService;
        _options1 = options.Get("SampleWorker");
        _options2 = options.Get("SampleWorker2");
    }

    public async Task OnGet()
    {
        var queueFullname1 = await _isqsQueueService.GetQueueFullNameAsync(_options1.QueueName);
        var queueFullname2 = await _isqsQueueService.GetQueueFullNameAsync(_options2.QueueName);
        await _isqsQueueService.EnqueueMessageAsync(queueFullname1!, "Started Queue1 !!!!");
        await _isqsQueueService.EnqueueMessageAsync(queueFullname2!, "Started Queue2 !!!!");
    }
    
    public async Task OnPost()
    {
        var message1 = Request.Form["message1"].ToString();
        var message2 = Request.Form["message2"].ToString();

       
        
        
        if (!string.IsNullOrEmpty(message1))
        {
            var queueFullname1 = await _isqsQueueService.GetQueueFullNameAsync(_options1.QueueName);
            if (!string.IsNullOrEmpty(queueFullname1))
                await _isqsQueueService.EnqueueMessageAsync(queueFullname1, message1);
        }
       
        if (!string.IsNullOrEmpty(message2))
        {
            var queueFullname2 = await _isqsQueueService.GetQueueFullNameAsync(_options2.QueueName);
            if (!string.IsNullOrEmpty(queueFullname2))
                await _isqsQueueService.EnqueueMessageAsync(queueFullname2, message2);
        }

    }
}