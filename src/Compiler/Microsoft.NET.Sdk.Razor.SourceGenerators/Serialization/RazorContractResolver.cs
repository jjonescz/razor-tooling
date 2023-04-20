using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal sealed class RazorContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (property.DeclaringType == typeof(LazyIntermediateToken))
        {
            property.Ignored = true;
        }

        return property;
    }

    private class IntermediateNodeTypeProvider : IValueProvider
    {
        public static readonly IntermediateNodeTypeProvider Instance = new();

        private IntermediateNodeTypeProvider() { }

        public object? GetValue(object target)
        {
            return target.GetType().Name;
        }

        public void SetValue(object target, object? value)
        {
            throw new NotSupportedException();
        }
    }
}
