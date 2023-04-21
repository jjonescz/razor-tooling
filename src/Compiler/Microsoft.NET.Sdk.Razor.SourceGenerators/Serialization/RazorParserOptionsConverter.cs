using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Newtonsoft.Json;
using System;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal sealed class RazorParserOptionsConverter : JsonConverter<RazorParserOptions>
{
    private const string ValuePropertyName = "_";

    public override RazorParserOptions? ReadJson(JsonReader reader, Type objectType, RazorParserOptions? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        var designTime = reader.ReadPropertyName(nameof(RazorParserOptions.DesignTime)).ReadAsBoolean().GetValueOrDefault();
        reader.ReadPropertyName(ValuePropertyName).Read();
        return designTime ? RazorParserOptions.CreateDesignTime(factory) : RazorParserOptions.Create(factory);

        void factory(RazorParserOptionsBuilder builder)
        {
            serializer.Populate(reader, builder);
        }
    }

    public override void WriteJson(JsonWriter writer, RazorParserOptions? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();
        writer.WritePropertyName(nameof(RazorParserOptions.DesignTime));
        writer.WriteValue(value.DesignTime);
        writer.WritePropertyName(ValuePropertyName);
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(RazorParserOptions.ParseLeadingDirectives));
        writer.WriteValue(value.ParseLeadingDirectives);
        writer.WritePropertyName(nameof(RazorParserOptions.Version));
        writer.WriteValue(value.Version.ToString());
        writer.WritePropertyName(nameof(RazorParserOptions.FileKind));
        writer.WriteValue(value.FileKind);
        writer.WritePropertyName(nameof(RazorParserOptions.Directives));
        writer.WriteStartArray();

        foreach (var directive in value.Directives)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(DirectiveDescriptor.Description));
            writer.WriteValue(directive.Description);
            writer.WritePropertyName(nameof(DirectiveDescriptor.Directive));
            writer.WriteValue(directive.Directive);
            writer.WritePropertyName(nameof(DirectiveDescriptor.DisplayName));
            writer.WriteValue(directive.DisplayName);
            writer.WritePropertyName(nameof(DirectiveDescriptor.Kind));
            writer.WriteValue(directive.Kind.ToString());
            writer.WritePropertyName(nameof(DirectiveDescriptor.Usage));
            writer.WriteValue(directive.Usage.ToString());
            writer.WritePropertyName(nameof(DirectiveDescriptor.Tokens));
            writer.WriteStartArray();

            foreach (var token in directive.Tokens)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(nameof(DirectiveTokenDescriptor.Kind));
                writer.WriteValue(token.Kind);
                writer.WritePropertyName(nameof(DirectiveTokenDescriptor.Optional));
                writer.WriteValue(token.Optional);
                writer.WritePropertyName(nameof(DirectiveTokenDescriptor.Name));
                writer.WriteValue(token.Name);
                writer.WritePropertyName(nameof(DirectiveTokenDescriptor.Description));
                writer.WriteValue(token.Description);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}
