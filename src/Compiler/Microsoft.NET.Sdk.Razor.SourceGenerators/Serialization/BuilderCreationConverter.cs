using Newtonsoft.Json;
using System;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal sealed class BuilderCreationConverter<T> : JsonConverter<T> where T : class
{
    private readonly Func<Action<object>, T> _factory;

    public BuilderCreationConverter(Func<Action<object>, T> factory)
    {
        _factory = factory;
    }

    public override bool CanWrite => false;

    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        return _factory(builder => serializer.Populate(reader, builder));
    }

    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
