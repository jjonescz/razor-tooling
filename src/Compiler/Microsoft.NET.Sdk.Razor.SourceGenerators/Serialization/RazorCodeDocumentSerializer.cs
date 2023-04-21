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
                new DelegateCreationConverter<TagHelperDocumentContext>(_ => TagHelperDocumentContext.Create("", Array.Empty<TagHelperDescriptor>())),
                new DelegateCreationConverter<RazorParserOptions>(_ => RazorParserOptions.CreateDefault()),
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
                    reader.Read();
                    document.SetTagHelperContext(_serializer.Deserialize<TagHelperDocumentContext>(reader));
                    break;
                case nameof(DocumentIntermediateNode):
                    reader.Read();
                    document.SetDocumentIntermediateNode(_serializer.Deserialize<DocumentIntermediateNode>(reader));
                    break;
                case nameof(ParserOptions):
                    reader.Read();
                    document.SetParserOptions(_serializer.Deserialize<RazorParserOptions>(reader));
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
            _serializer.Serialize(writer, tagHelperContext);
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
            Serialize(writer, syntaxTree.Options);

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

    private void Serialize(JsonWriter writer, RazorParserOptions parserOptions)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(RazorParserOptions.DesignTime));
        writer.WriteValue(parserOptions.DesignTime);
        writer.WritePropertyName(nameof(RazorParserOptions.ParseLeadingDirectives));
        writer.WriteValue(parserOptions.ParseLeadingDirectives);
        writer.WritePropertyName(nameof(RazorParserOptions.Version));
        writer.WriteValue(parserOptions.Version.ToString());
        writer.WritePropertyName(nameof(RazorParserOptions.FileKind));
        writer.WriteValue(parserOptions.FileKind);
        writer.WritePropertyName(nameof(RazorParserOptions.Directives));
        writer.WriteStartArray();

        foreach (var directive in parserOptions.Directives)
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
        Serialize(writer, syntaxTree.Options);

        writer.WritePropertyName(nameof(RazorSyntaxTree.Source));
        Serialize(writer, syntaxTree.Source);

        writer.WriteEndObject();
    }
}
