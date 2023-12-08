﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Language;

internal abstract class RazorHtmlDocument : IRazorGeneratedDocument
{
    public abstract string GeneratedCode { get; }

    public abstract RazorCodeGenerationOptions Options { get; }

    public abstract ImmutableArray<SourceMapping> SourceMappings { get; }

    public ImmutableArray<SourceSpan> GeneratedOnlyMappings => ImmutableArray<SourceSpan>.Empty;

    public abstract RazorCodeDocument CodeDocument { get; }

    public static RazorHtmlDocument Create(RazorCodeDocument codeDocument, string generatedHtml, RazorCodeGenerationOptions options, ImmutableArray<SourceMapping> sourceMappings)
    {
        if (generatedHtml == null)
        {
            throw new ArgumentNullException(nameof(generatedHtml));
        }

        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        return new DefaultRazorHtmlDocument(codeDocument, generatedHtml, options, sourceMappings);
    }
}
