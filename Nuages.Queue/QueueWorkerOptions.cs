using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Nuages.Queue;

[ExcludeFromCodeCoverage]
public class QueueWorkerOptions
{
    public bool Enabled { get; set; } = true;
    
    public int MaxMessagesCount { get; set; } = 10;
    public int WaitDelayInMillisecondsWhenNoMessages { get; set; } = 1000;
    
    [Required] public string QueueName { get; set; } = null!;
}