using System.Threading.Tasks;
using ArtifactsApiClient.Models;
using ArtifactsBackend.Behavior.Base;
using ArtifactsBackend.Core;

namespace ArtifactsBackend.Character;

public class ArtifactsCharacter(CharacterSchema characterSchema)
{
    public string? Name => m_CharacterSchema.Name;
    public int? X => m_CharacterSchema.X;
    public int? Y => m_CharacterSchema.Y;

    public void StartBehavior(BaseBehaviorController behaviorController)
    {
        m_BehaviorController = behaviorController;
        Task.Run(m_BehaviorController.Start);
    }

    public async Task<ArtifactsHelper.Response<CharacterMovementResponseSchema?>> Move(int x, int y)
    {
        ArtifactsHelper.Response<CharacterMovementResponseSchema?> result = await ArtifactsHelper.Client
            .My[Name].Action.Move
            .PostAsync(new DestinationSchema { X = x, Y = y }).TryGetResult();

        if (result.Success)
        {
            m_CharacterSchema = result.Result!.Data!.Character!;
        }

        return result;
    }


    private BaseBehaviorController? m_BehaviorController;
    private CharacterSchema m_CharacterSchema = characterSchema;
}