using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ArtifactsApiClient.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace ArtifactsBackend.Core;

public static class ArtifactsHelper
{
    public static ArtifactsApiClient.ArtifactsApiClient Client { get; }

    public record struct Response<TResult>(TResult Result, bool Success, int StatusCode, string? Error);

    public static async Task<Response<TResult?>> TryGetResult<TResult>(this Task<TResult?> task)
    {
        try
        {
            TResult? result = await task;
            return result is not null
                ? new Response<TResult?>(result, true, (int)HttpStatusCode.OK, null)
                : new Response<TResult?>(default, false, (int)HttpStatusCode.BadRequest, "Result is null!");
        }
        catch (ApiException e)
        {
            return new Response<TResult?>(default, false, e.ResponseStatusCode, e.Message);
        }
    }

    public static async Task WaitCooldown(this CooldownSchema cooldown)
    {
        await Task.Delay(TimeSpan.FromSeconds(cooldown.RemainingSeconds ?? 0));
    }

    static ArtifactsHelper()
    {
        ApiKeyAuthenticationProvider authProvider = new(
            $"Bearer {File.ReadAllText("ApiToken.txt")}",
            "Authorization",
            ApiKeyAuthenticationProvider.KeyLocation.Header,
            "api.artifactsmmo.com"
        );

        HttpClientRequestAdapter requestAdapter = new(authProvider)
        {
            BaseUrl = "https://api.artifactsmmo.com"
        };

        Client = new ArtifactsApiClient.ArtifactsApiClient(requestAdapter);
    }
}