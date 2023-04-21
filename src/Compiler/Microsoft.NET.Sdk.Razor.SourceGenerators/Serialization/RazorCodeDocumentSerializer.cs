// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal sealed class RazorCodeDocumentSerializer
{
    private const string TagHelperContext = nameof(TagHelperContext);
    private const string ParserOptions = nameof(ParserOptions);
    private const string Imports = nameof(Imports);
    private const string SyntaxTree = nameof(SyntaxTree);
    private const string Content = nameof(Content);
    private const string DocumentIntermediateNode = nameof(DocumentIntermediateNode);

    private readonly JsonSerializer _serializer;

    public static readonly RazorCodeDocumentSerializer Instance = new();

    private RazorCodeDocumentSerializer()
    {
        _serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented, // TODO: Remove.
            Converters =
            {
                RazorDiagnosticJsonConverter.Instance,
                TagHelperDescriptorJsonConverter.Instance,
                new EncodingConverter(),
                new DelegateCreationConverter<RazorCodeGenerationOptions>(_ => RazorCodeGenerationOptions.CreateDefault()),
                new SourceSpanConverter(),
                new RazorParserOptionsConverter(),
                new DirectiveDescriptorConverter(),
                new DirectiveTokenDescriptorConverter(),
            },
            ContractResolver = new RazorContractResolver(),
            TypeNameHandling = TypeNameHandling.Auto,
            SerializationBinder = new RazorSerializationBinder(),
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
        };
    }

    public RazorCodeDocument? Deserialize(string json, RazorSourceDocument source)
    {
        using var textReader = new StringReader(json);
        using var jsonReader = new JsonTextReader(textReader);
        return Deserialize(jsonReader, source);
    }

    public RazorCodeDocument? Deserialize(JsonReader reader, RazorSourceDocument source)
    {
        if (!reader.Read() || reader.TokenType != JsonToken.StartObject)
        {
            return null;
        }

        var document = RazorCodeDocument.Create(source);

        reader.ReadProperties(propertyName =>
        {
            switch (propertyName)
            {
                case nameof(TagHelperContext):
                    if (reader.Read() && reader.TokenType == JsonToken.StartObject)
                    {
                        string? prefix = null;
                        IReadOnlyList<TagHelperDescriptor>? tagHelpers = null;
                        reader.ReadProperties(propertyName =>
                        {
                            switch (propertyName)
                            {
                                case nameof(TagHelperDocumentContext.Prefix) when reader.Read():
                                    prefix = (string?)reader.Value;
                                    break;
                                case nameof(TagHelperDocumentContext.TagHelpers) when reader.Read():
                                    tagHelpers = _serializer.Deserialize<IReadOnlyList<TagHelperDescriptor>?>(reader);
                                    break;
                            }
                        });
                        if (tagHelpers != null)
                        {
                            document.SetTagHelperContext(TagHelperDocumentContext.Create(prefix, tagHelpers));
                        }
                    }
                    break;
                case nameof(ParserOptions):
                    reader.Read();
                    document.SetParserOptions(_serializer.Deserialize<RazorParserOptions>(reader));
                    break;
                case nameof(DocumentIntermediateNode):
                    reader.Read();
                    document.SetDocumentIntermediateNode(_serializer.Deserialize<DocumentIntermediateNode>(reader));
                    break;
            }
        });

        return document;
    }

    public string Serialize(RazorCodeDocument? document)
    {
        using var textWriter = new StringWriter();
        using var jsonWriter = new JsonTextWriter(textWriter);
        Serialize(jsonWriter, document);
        return textWriter.ToString();
    }

    public void Serialize(JsonWriter writer, RazorCodeDocument? document)
    {
        if (document == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();

        if (document.GetTagHelperContext() is { } tagHelperContext)
        {
            writer.WritePropertyName(TagHelperContext);
            writer.WriteStartObject();

            if (tagHelperContext.Prefix is { } prefix)
            {
                writer.WritePropertyName(nameof(TagHelperDocumentContext.Prefix));
                writer.WriteValue(prefix);
            }

            if (tagHelperContext.TagHelpers is { Count: > 0 } tagHelpers)
            {
                writer.WritePropertyName(nameof(TagHelperDocumentContext.TagHelpers));
                _serializer.Serialize(writer, tagHelpers);
            }

            writer.WriteEndObject();
        }

        if (document.GetParserOptions() is { } parserOptions)
        {
            writer.WritePropertyName(ParserOptions);
            _serializer.Serialize(writer, parserOptions);
        }

        if (false && document.GetSyntaxTree() is { } syntaxTree)
        {
            writer.WritePropertyName(SyntaxTree);
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(RazorSyntaxTree.Options));
            _serializer.Serialize(writer, syntaxTree.Options);

            if (syntaxTree.Source != document.Source)
            {
                Debug.Assert(false);
                writer.WritePropertyName(nameof(RazorSyntaxTree.Source));
                Serialize(writer, syntaxTree.Source);
            }

            writer.WriteEndObject();
        }

        if (false && document.GetImportSyntaxTrees() is { Count: > 0 } imports)
        {
            writer.WritePropertyName(Imports);
            writer.WriteStartArray();

            foreach (var importSyntaxTree in imports)
            {
                writer.WriteValue(importSyntaxTree.Source.FilePath);
            }

            writer.WriteEndArray();
        }

        if (document.GetDocumentIntermediateNode() is { } intermediateNode)
        {
            writer.WritePropertyName(DocumentIntermediateNode);
            _serializer.Serialize(writer, intermediateNode);
        }

        writer.WriteEndObject();
    }

    private void Serialize(JsonWriter writer, RazorSourceDocument source)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(RazorSourceDocument.Encoding));
        writer.WriteValue(source.Encoding.WebName);
        writer.WritePropertyName(nameof(RazorSourceDocument.FilePath));
        writer.WriteValue(source.FilePath);
        writer.WritePropertyName(nameof(RazorSourceDocument.RelativePath));
        writer.WriteValue(source.RelativePath);
        writer.WritePropertyName(Content);

        var content = ArrayPool<char>.Shared.Rent(source.Length);
        source.CopyTo(0, content, 0, source.Length);

        using (StringBuilderPool.GetPooledObject(out var stringBuilder))
        {
            stringBuilder.EnsureCapacity(source.Length);
            stringBuilder.Append(content);
            writer.WriteValue(stringBuilder.ToString());
        }

        ArrayPool<char>.Shared.Return(content);

        writer.WriteEndObject();
    }

    private void Serialize(JsonWriter writer, RazorSyntaxTree syntaxTree)
    {
        writer.WriteStartObject();

        writer.WritePropertyName(nameof(RazorSyntaxTree.Options));
        _serializer.Serialize(writer, syntaxTree.Options);

        writer.WritePropertyName(nameof(RazorSyntaxTree.Source));
        Serialize(writer, syntaxTree.Source);

        writer.WriteEndObject();
    }

    private static BuilderCreationConverter<T> BuilderConverter<T>(Func<Action<object>, T> factory) where T : class
    {
        return new BuilderCreationConverter<T>(factory);
    }
}
