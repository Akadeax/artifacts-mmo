using System.Threading.Tasks;
using ArtifactsApiClient.Models;
using ArtifactsBackend.Behavior.Base;
using ArtifactsBackend.Core;

namespace ArtifactsBackend.Character;

public class ArtifactsCharacter(CharacterSchema characterSchema)
{
    public string? Name => characterSchema.Name;
    public int? X => characterSchema.X;
    public int? Y => characterSchema.Y;

    public void StartBehavior(BaseBehaviorController behaviorController)
    {
        m_BehaviorController = behaviorController;
        Task.Run(m_BehaviorController.Start);
    }

    public async Task<ArtifactsHelper.Response<CharacterMovementResponseSchema?>> Move(int x, int y)
    {
        ArtifactsHelper.Response<CharacterMovementResponseSchema?> result = await ArtifactsHelper.Client
            .My[Name].Action.Move
            .PostAsync(new DestinationSchema { MapId = 319 }).TryGetResult();

        if (result.Success)
        {
            characterSchema.X = x;
            characterSchema.Y = y;
        }

        return result;
    }


    private BaseBehaviorController? m_BehaviorController;
}