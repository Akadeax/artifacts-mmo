using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lua;

namespace ArtifactsBackend.Behavior.Simple.Behaviors;

public class SimpleLuaBehavior(string filePath, IDictionary<string, LuaValue> parameters) : SimpleBehavior
{
    /// <inheritdoc />
    protected override async Task<Result> RunInternal()
    {
        LuaState state = LuaState.Create();
        foreach ((string paramName, LuaValue paramValue) in parameters)
        {
            state.Environment[new LuaValue(paramName)] = paramValue;
        }

        // await state.DoFileAsync()
        return Result.Success;
    }
}