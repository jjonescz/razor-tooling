// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Newtonsoft.Json;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators;

internal sealed class RazorCodeDocumentSerializer
{
    private const string TagHelperContext = nameof(TagHelperContext);
    private const string ParserOptions = nameof(ParserOptions);
    private const string Imports = nameof(Imports);
    private const string SyntaxTree = nameof(SyntaxTree);
    private const string Content = nameof(Content);
    private const string DocumentIntermediateNode = nameof(DocumentIntermediateNode);
    private const string FileKind = nameof(FileKind);
    private const string CssScope = nameof(CssScope);

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
                new RazorCodeGenerationOptionsConverter(),
                new SourceSpanConverter(),
                new RazorParserOptionsConverter(),
                new DirectiveDescriptorConverter(),
                new DirectiveTokenDescriptorConverter(),
                new ItemCollectionConverter(),
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
                case nameof(FileKind):
                    document.SetFileKind(reader.ReadAsString());
                    break;
                case nameof(CssScope):
                    document.SetCssScope(reader.ReadAsString());
                    break;
                case nameof(TagHelperContext):
                    if (reader.Read() && reader.TokenType == JsonToken.StartObject)
                    {
                        string? prefix = null;
                        IReadOnlyList<TagHelperDescriptor>? tagHelpers = null;
                        reader.ReadProperties(propertyName =>
                        {
                            switch (propertyName)
                            {
                                case nameof(TagHelperDocumentContext.Prefix):
                                    reader.Read();
                                    prefix = (string?)reader.Value;
                                    break;
                                case nameof(TagHelperDocumentContext.TagHelpers):
                                    reader.Read();
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
                case nameof(SyntaxTree):
                    if (reader.Read() && DeserializeSyntaxTree(reader, document) is { } syntaxTree)
                    {
                        document.SetSyntaxTree(syntaxTree);
                    }
                    break;
                case nameof(Imports):
                    if (reader.Read() && reader.TokenType == JsonToken.StartArray)
                    {
                        using var _ = ArrayBuilderPool<RazorSyntaxTree>.GetPooledObject(out var importTrees);
                        while (reader.Read() && DeserializeSyntaxTree(reader, document) is { } importTree)
                        {
                            importTrees.Add(importTree);
                        }
                        document.SetImportSyntaxTrees(importTrees.ToImmutable());
                    }
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

        if (document.GetFileKind() is { } fileKind)
        {
            writer.WritePropertyName(nameof(FileKind));
            writer.WriteValue(fileKind);
        }

        if (document.GetCssScope() is { } cssScope)
        {
            writer.WritePropertyName(nameof(CssScope));
            writer.WriteValue(cssScope);
        }

        if (document.GetDocumentIntermediateNode() is { } intermediateNode)
        {
            writer.WritePropertyName(DocumentIntermediateNode);
            _serializer.Serialize(writer, intermediateNode);
        }

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

        if (document.GetSyntaxTree() is { } syntaxTree)
        {
            writer.WritePropertyName(SyntaxTree);
            SerializeSyntaxTree(writer, document, syntaxTree);
        }

        if (document.GetImportSyntaxTrees() is { Count: > 0 } imports)
        {
            writer.WritePropertyName(Imports);
            writer.WriteStartArray();

            foreach (var importSyntaxTree in imports)
            {
                SerializeSyntaxTree(writer, document, importSyntaxTree);
            }

            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }

    private void SerializeSourceDocument(JsonWriter writer, RazorSourceDocument source)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(RazorSourceDocument.Encoding));
        _serializer.Serialize(writer, source.Encoding);
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

    private RazorSourceDocument? DeserializeSourceDocument(JsonReader reader)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            return null;
        }

        Encoding? encoding = null;
        string? filePath = null;
        string? relativePath = null;
        string? content = null;
        reader.ReadProperties(propertyName =>
        {
            switch (propertyName)
            {
                case nameof(RazorSourceDocument.Encoding):
                    encoding = _serializer.Deserialize<Encoding>(reader);
                    break;
                case nameof(RazorSourceDocument.FilePath):
                    filePath = reader.ReadAsString();
                    break;
                case nameof(RazorSourceDocument.RelativePath):
                    relativePath = reader.ReadAsString();
                    break;
                case nameof(Content):
                    content = reader.ReadAsString();
                    break;
            }
        });
        return RazorSourceDocument.Create(content, encoding, new RazorSourceDocumentProperties(filePath, relativePath));
    }

    private void SerializeSyntaxTree(JsonWriter writer, RazorCodeDocument owner, RazorSyntaxTree syntaxTree)
    {
        writer.WriteStartObject();

        if (syntaxTree.Options != owner.GetParserOptions())
        {
            writer.WritePropertyName(nameof(RazorSyntaxTree.Options));
            _serializer.Serialize(writer, syntaxTree.Options);
        }

        if (syntaxTree.Source != owner.Source)
        {
            writer.WritePropertyName(nameof(RazorSyntaxTree.Source));
            SerializeSourceDocument(writer, syntaxTree.Source);
        }

        writer.WriteEndObject();
    }

    private RazorSyntaxTree? DeserializeSyntaxTree(JsonReader reader, RazorCodeDocument owner)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            return null;
        }

        RazorParserOptions? options = owner.GetParserOptions();
        RazorSourceDocument source = owner.Source;
        reader.ReadProperties(propertyName =>
        {
            switch (propertyName)
            {
                case nameof(RazorSyntaxTree.Options):
                    reader.Read();
                    options = _serializer.Deserialize<RazorParserOptions>(reader);
                    break;
                case nameof(RazorSyntaxTree.Source):
                    reader.Read();
                    source = DeserializeSourceDocument(reader)!;
                    break;
            }
        });
        return RazorSyntaxTree.Parse(source, options);
    }
}
