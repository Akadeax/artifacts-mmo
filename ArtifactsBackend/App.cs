using System;
using System.Linq;
using System.Threading.Tasks;
using ArtifactsApiClient.Models;
using ArtifactsBackend.Behavior;
using ArtifactsBackend.Behavior.Base;
using ArtifactsBackend.Behavior.ConfigurationImplementations;
using ArtifactsBackend.Core;
using ArtifactsBackend.Character;
using Spectre.Console;

namespace ArtifactsBackend;

public class App
{
    public void Stop()
    {
        m_Running = false;
    }

    public void Run()
    {
        Console.CancelKeyPress += (_, _) => Stop();

        RegisterCharacters().GetAwaiter().GetResult();

        while (m_Running)
        {
        }
    }

    private async Task RegisterCharacters()
    {
        CharacterMovementResponseSchema? result =
            await ArtifactsHelper.Client.My["AkaMain"].Action.Move.PostAsync(new DestinationSchema { X = 0, Y = 7 });
        Console.WriteLine($"Finished gathering, loc: {result.Data.Character.Y}");

        return;
        var response = await ArtifactsHelper.Client.My.Characters.GetAsync().TryGetResult();
        if (!response.Success || response.Result is null)
        {
            ConsoleHelper.Error.Fatal(response);
            return;
        }

        if (response.Result.Data is not { } characters || characters.None())
        {
            ConsoleHelper.Error.Fatal("You do not have any characters! Create at least 1 and re-launch.");
            Stop();
            return;
        }

        ConsoleHelper.Info.Print($"You have {characters.Count} character(s). Configuring them...");


        foreach (ArtifactsCharacter character in characters.Select(characterSchema => new ArtifactsCharacter(characterSchema)))
        {
            var prompt = new SelectionPrompt<string>()
                .Title($"Select task for {character.Name}:")
                .AddChoices(BehaviorControllerManager.BehaviorControllerConfigurations.Select(cfg => cfg!.Name));

            string choice = await AnsiConsole.PromptAsync(prompt);

            IBehaviorControllerConfiguration behaviorConfig = BehaviorControllerManager.BehaviorControllerConfigurations
                .First(cfg => cfg!.Name == choice) ?? throw new InvalidOperationException();
            character.StartBehavior(behaviorConfig.Build(character));
        }

        ConsoleHelper.Info.Print($"Configured! You now have {characters.Count} characters running.");
    }

    private bool m_Running = true;
}