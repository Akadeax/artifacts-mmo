using System.Collections.Generic;
using System.Linq;

namespace ArtifactsBackend.Core;

public static class LinqExtensions
{
    public static bool None<T>(this IEnumerable<T> enumerable)
    {
        return !enumerable.Any();
    }
}