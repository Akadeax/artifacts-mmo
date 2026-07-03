using System.Threading.Tasks;
using ArtifactsBackend.Character;

namespace ArtifactsBackend.Behavior.Base;

public abstract class BaseBehaviorController(ArtifactsCharacter character)
{
    public ArtifactsCharacter Character => character;

    public virtual async Task Start() => await Task.CompletedTask;
    public virtual async Task Stop() => await Task.CompletedTask;
}