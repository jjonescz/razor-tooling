using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal sealed class IntermediateNodeConverter : JsonConverter<IntermediateNode>
{
    public const string TypePropertyName = "$type";

    private readonly Assembly _assembly = typeof(IntermediateNode).Assembly;

    public override bool CanWrite => false;

    public override IntermediateNode? ReadJson(JsonReader reader, Type objectType, IntermediateNode? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        reader.ReadPropertyName(TypePropertyName);
        var typeName = reader.ReadAsString();
        var typeFullName = "Microsoft.AspNetCore.Razor.Language.Intermediate." + typeName;
        var type = _assembly.GetType(typeFullName, throwOnError: true);
        var node = (IntermediateNode)Activator.CreateInstance(type);
        serializer.Populate(reader, node);
        return node;
    }

    public override void WriteJson(JsonWriter writer, IntermediateNode? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
