using System;

namespace ArtifactsBackend.Core;

[AttributeUsage(AttributeTargets.Class)]
public class RegisterAttribute(string registryType) : Attribute
{
    public string RegistryType => registryType;
}