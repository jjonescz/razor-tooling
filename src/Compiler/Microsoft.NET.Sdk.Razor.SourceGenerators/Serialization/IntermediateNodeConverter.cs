using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Reflection;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal sealed class IntermediateNodeConverter : JsonConverter<IntermediateNode>
{
    private readonly Assembly _assembly = typeof(IntermediateNode).Assembly;

    public override IntermediateNode? ReadJson(JsonReader reader, Type objectType, IntermediateNode? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        reader.Read();
        if (reader.TokenType != JsonToken.PropertyName)
        {
            return null;
        }

        var typeName = (string)reader.Value!;
        var typeFullName = "Microsoft.AspNetCore.Razor.Language.Intermediate." + typeName;
        var type = _assembly.GetType(typeFullName, throwOnError: true);
        var node = (IntermediateNode)Activator.CreateInstance(type);
        reader.Read();
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
        writer.WritePropertyName(value.GetType().Name);

        serializer.EqualityComparer ??= new IgnoringEqualityComparer();
        var comparer = (IgnoringEqualityComparer)serializer.EqualityComparer;
        comparer.Ignore = value;
        serializer.Serialize(writer, value);

        writer.WriteEndObject();
    }

    private class IgnoringEqualityComparer : IEqualityComparer
    {
        public object? Ignore { get; set; }

        public new bool Equals(object x, object y)
        {
            if (Ignore is not null && ReferenceEquals(x, Ignore))
            {
                return false;
            }
            return object.Equals(x, y);
        }

        public int GetHashCode(object obj)
        {
            if (Ignore is not null && ReferenceEquals(obj, Ignore))
            {
                return -1;
            }
            return obj?.GetHashCode() ?? 0;
        }
    }
}
