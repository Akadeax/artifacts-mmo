using System;
using System.Threading.Tasks;

namespace ArtifactsBackend.Behavior.Simple;

public class SimpleNullBehavior : SimpleBehavior
{
    /// <inheritdoc />
    protected override async Task<Result> RunInternal()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return Result.Success;
    }
}