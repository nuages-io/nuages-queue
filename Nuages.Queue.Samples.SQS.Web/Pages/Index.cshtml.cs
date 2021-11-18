using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Nuages.Queue.SQS;
// ReSharper disable UnusedMember.Global

namespace Nuages.Queue.Samples.SQS.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ISQSQueueService _isqsQueueService;
    private readonly QueueWorkerOptions _options;

    public IndexModel( ISQSQueueService isqsQueueService, IOptions<QueueWorkerOptions> options)
    {
        _isqsQueueService = isqsQueueService;
        _options = options.Value;
    }

    public async Task OnGet()
    {
        var  fullName = await _isqsQueueService.GetQueueFullNameAsync(_options.QueueName);
        await _isqsQueueService.EnqueueMessageAsync(fullName!, "Started !!!!");

    }
    
    public async Task OnPost()
    {
        var message = Request.Form["message"];
        
        var  fullName = await _isqsQueueService.GetQueueFullNameAsync(_options.QueueName);
        await _isqsQueueService.EnqueueMessageAsync(fullName!, message);

    }
}