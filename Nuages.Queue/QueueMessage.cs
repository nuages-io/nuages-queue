namespace Nuages.Queue;

public class QueueMessage
{
    public string MessageId { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Handle { get; set; } = string.Empty;
}