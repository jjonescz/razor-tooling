using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Newtonsoft.Json;
using System;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal sealed class IntermediateNodeConverter : JsonConverter<IntermediateNode>
{
    private const string TypeProperty = "$";
    private const string ValueProperty = "_";

    public override IntermediateNode? ReadJson(JsonReader reader, Type objectType, IntermediateNode? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        reader.ReadPropertyName(TypeProperty);
        var type = reader.ReadAsString();
        var node = (IntermediateNode)Activator.CreateInstance(Type.GetType("Microsoft.AspNetCore.Razor.Language.Intermediate." + type));
        reader.ReadPropertyName(ValueProperty);
        serializer.Populate(reader, node);
        return node;
    }

    public override void WriteJson(JsonWriter writer, IntermediateNode? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();
        writer.WritePropertyName(TypeProperty);
        writer.WriteValue(value.GetType().Name);
        writer.WritePropertyName(ValueProperty);
        serializer.Converters.Remove(this);
        serializer.Serialize(writer, value);
        serializer.Converters.Add(this);
        writer.WriteEndObject();
    }
}
