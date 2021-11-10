using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Nuages.Queue.ASQ;

[ExcludeFromCodeCoverage]
// ReSharper disable once InconsistentNaming
public class ASQQueueClientOptions
{
    [Required] public string ConnectionString { get; set; } = null!;
}