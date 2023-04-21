using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        if (property.DeclaringType == typeof(DocumentIntermediateNode) && property.PropertyName == nameof(DocumentIntermediateNode.Target))
        {
            property.Ignored = true;
        }

        return property;
    }
}
