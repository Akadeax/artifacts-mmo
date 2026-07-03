using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtifactsApiClient.Models;
using ArtifactsBackend.Core;

namespace ArtifactsBackend.Behavior.Simple.Behaviors;

public sealed class SimpleGatherBehavior(int gatherSpotX, int gatherSpotY) : SimpleBehavior
{
    private int yLoc = 6;

    /// <inheritdoc />
    protected override async Task<Result> RunInternal()
    {
        ConsoleHelper.Info.Print("Starting gathering...");

        try
        {
            CharacterMovementResponseSchema? result =
                await ArtifactsHelper.Client.My[Controller!.Character.Name].Action.Move.PostAsync(new DestinationSchema { X = 0, Y = yLoc });
            Console.WriteLine($"Finished gathering, loc: {result.Data.Character.Y}");
        }
        catch (ErrorResponseSchema e)
        {
            Console.WriteLine($"Schema error: {e.Error.Message}");
            await Task.Delay(6000);
            return Result.Success;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Regular error: {e.Message}");
            await Task.Delay(6000);

            return Result.Success;
        }

        await Task.Delay(6000);
        yLoc++;

        ConsoleHelper.Info.Print("Finished gathering");
        return Result.Success;
        // if (Controller!.Character.X != gatherSpotX || Controller.Character.Y != gatherSpotY)
        // {
        //     ConsoleHelper.Info.Print("Move to gathering spot is required.");
        //
        //     Console.WriteLine("premove");
        //     ArtifactsHelper.Response<CharacterMovementResponseSchema?> moveResponse = await Controller.Character.Move(gatherSpotX, gatherSpotY);
        //     Console.WriteLine("postmove");
        //     
        //     if (!moveResponse.Success)
        //     {
        //         ConsoleHelper.Error.NonFatal(moveResponse);
        //         return Result.Failure;
        //     }
        //
        //     MapSchema destination = moveResponse.Result!.Data!.Destination!;
        //     int? moveCooldown = moveResponse.Result!.Data!.Cooldown!.TotalSeconds;
        //     ConsoleHelper.Info.Print($"Successfully moved to gathering location at ({destination.X}, {destination.Y})! Cooldown: {moveCooldown}");
        //     await moveResponse.Result!.Data!.Cooldown!.WaitCooldown();
        // }
        //
        // ArtifactsHelper.Response<SkillResponseSchema?> skillResponse = await ArtifactsHelper
        //     .Client.My[Controller.Character.Name].Action.Gathering
        //     .PostAsync().TryGetResult();
        //
        // ConsoleHelper.Info.Print("Gather happening now!");
        // if (!skillResponse.Success)
        // {
        //     ConsoleHelper.Error.NonFatal(skillResponse);
        //     return Result.Failure;
        // }
        //
        // IEnumerable<string?> itemNames = skillResponse.Result!.Data!.Details!.Items!.Select(item => $"{item.Code} ({item.Quantity})");
        // int? actionCooldown = skillResponse.Result!.Data!.Cooldown!.TotalSeconds;
        // ConsoleHelper.Info.Print($"Successfully harvested [{string.Join(';', itemNames)}]! Cooldown: {actionCooldown}");
        // await skillResponse.Result!.Data!.Cooldown!.WaitCooldown();
        //
        // return Result.Success;
    }
}