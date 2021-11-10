using System.Diagnostics.CodeAnalysis;

namespace Nuages.Queue.SQS;

[ExcludeFromCodeCoverage]
public class QueueOptions
{
    public bool AutoCreateQueue { get; set; } = true;
}