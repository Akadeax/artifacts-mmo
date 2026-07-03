using System.Collections.Generic;
using Spectre.Console;

namespace ArtifactsBackend.Core;

public static class ConsoleHelper
{
    public static class Info
    {
        public static void Print(string message)
        {
            AnsiConsole.MarkupLineInterpolated($"[blue]🛈[/] {message}");
        }
    }

    public static class Error
    {
        private static readonly Dictionary<int, string> s_ErrorCodes = new()
        {
            { 452, "Invalid token. Please check the token and try again." },
            { 422, "Invalid payload." },
            { 486, "An action is already in progress." },
            { 490, "The character is already at the destination." },
            { 493, "Skill level too low." },
            { 497, "Inventory full." },
            { 498, "Character not found." },
            { 499, "Character is on cooldown." },


            { 500, "Internal server error." },
            { 502, "Bad Gateway. The Artifacts server is probably down. See the discord for info." },
            { 598, "There is no resource on this map." }
        };

        public static void NonFatal<T>(ArtifactsHelper.Response<T> response)
        {
            NonFatal($"Code {response.StatusCode} - {GetErrorStatusText(response.StatusCode)}");
        }

        public static void Fatal<T>(ArtifactsHelper.Response<T> response)
        {
            Fatal($"Code {response.StatusCode} - {GetErrorStatusText(response.StatusCode)}");
        }

        public static void NonFatal(string message)
        {
            AnsiConsole.MarkupLineInterpolated($"[Red]Error:[/] {message}");
        }

        public static void Fatal(string message)
        {
            AnsiConsole.MarkupLineInterpolated($"[bold DarkRed_1]✗ Fatal Error:[/] {message}");
        }

        private static string GetErrorStatusText(int statusCode)
        {
            return s_ErrorCodes.GetValueOrDefault(statusCode, "Unregistered status code. Please update ArtifactsBackend::Core::ConsoleHelper.");
        }
    }
}