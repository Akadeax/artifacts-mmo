using ArtifactsBackend.Behavior.Base;
using ArtifactsBackend.Behavior.Simple;
using ArtifactsBackend.Behavior.Simple.Behaviors;
using ArtifactsBackend.Character;
using ArtifactsBackend.Core;

namespace ArtifactsBackend.Behavior.ConfigurationImplementations;

[Register(IBehaviorControllerConfiguration.REGISTRY)]
public class ResourceTreeBCC : IBehaviorControllerConfiguration
{
    /// <inheritdoc />
    public string Name => nameof(ResourceTreeBCC);

    /// <inheritdoc />
    public BaseBehaviorController Build(ArtifactsCharacter character)
    {
        return new SimpleBehaviorController(character, [
            new SimpleGatherBehavior(-1, 0)
        ]);
    }
}