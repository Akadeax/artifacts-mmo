using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArtifactsBackend.Behavior.Base;
using ArtifactsBackend.Character;
using ArtifactsBackend.Core;

namespace ArtifactsBackend.Behavior.Simple;

public sealed class SimpleBehaviorController : BaseBehaviorController
{
    public SimpleBehaviorController(ArtifactsCharacter character,
        IReadOnlyCollection<SimpleBehavior> behaviors) : base(character)
    {
        m_Behaviors = behaviors;

        foreach (SimpleBehavior behavior in behaviors)
        {
            behavior.Initialize(this);
        }
    }

    /// <inheritdoc />
    public override async Task Start()
    {
        await m_RunningSemaphore.WaitAsync();

        m_Running = true;

        try
        {
            while (m_Running) await Evaluate();
        }
        finally
        {
            m_RunningSemaphore.Release();
        }
    }

    /// <inheritdoc />
    public override async Task Stop()
    {
        m_Running = false;
        await m_RunningSemaphore.WaitAsync();
        m_RunningSemaphore.Release();
    }

    private async Task Evaluate()
    {
        foreach (var behavior in m_Behaviors)
        {
            SimpleBehavior.Result result = await behavior.Run();
            if (result == SimpleBehavior.Result.Failure)
            {
                continue;
            }

            return;
        }

        ConsoleHelper.Error.NonFatal(
            $"SimpleBehaviorController of character {Character.Name} could not evaluate any action. All {m_Behaviors.Count} evaluated as \"Cannot Run\".");
    }

    private readonly SemaphoreSlim m_RunningSemaphore = new(1, 1);

    private bool m_Running;
    private readonly IReadOnlyCollection<SimpleBehavior> m_Behaviors;
}