using System.Diagnostics.CodeAnalysis;

namespace Nuages.Queue.ASQ;

[ExcludeFromCodeCoverage]
public class QueueOptions
{
    public bool AutoCreateQueue { get; set; } = true;
}