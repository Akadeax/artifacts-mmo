using ArtifactsBackend.Character;

namespace ArtifactsBackend.Behavior.Base;

public interface IBehaviorControllerConfiguration
{
    public const string REGISTRY = "BehaviorControllerConfiguration";

    public string Name { get; }

    /// <summary>
    /// Builds this Configuration into a concrete BehaviorController.
    /// </summary>
    /// <returns>The concrete BehaviorController</returns>
    public BaseBehaviorController Build(ArtifactsCharacter character);
}