using System.Threading.Tasks;
using ArtifactsBackend.Behavior.Base;
using ArtifactsBackend.Core;

namespace ArtifactsBackend.Behavior.Simple;

public abstract class SimpleBehavior : BaseBehavior
{
    public enum Result
    {
        Success,
        Failure
    }

    protected SimpleBehaviorController? Controller { get; private set; }

    /// <summary>
    /// Run this behavior. The behavior controller will re-evaluate from the start after.
    /// </summary>
    public async Task<Result> Run()
    {
        if (!m_Initialized)
        {
            ConsoleHelper.Error.NonFatal("Cannot run simple behavior without initializing!");
            return Result.Failure;
        }

        return await RunInternal();
    }

    protected abstract Task<Result> RunInternal();


    public void Initialize(SimpleBehaviorController controller)
    {
        if (m_Initialized)
        {
            ConsoleHelper.Error.NonFatal("Cannot initialize simple behavior twice!");
            return;
        }

        Controller = controller;
        m_Initialized = true;
    }

    private bool m_Initialized;
}