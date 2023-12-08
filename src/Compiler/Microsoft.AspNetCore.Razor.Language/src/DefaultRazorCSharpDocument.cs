﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRazorCSharpDocument : RazorCSharpDocument
{
    private readonly string _generatedCode;
    private readonly RazorDiagnostic[] _diagnostics;
    private readonly ImmutableArray<SourceMapping> _sourceMappings;
    private readonly ImmutableArray<SourceSpan> _generatedOnlyMappings;
    private readonly LinePragma[] _linePragmas;
    private readonly RazorCodeGenerationOptions _options;
    private readonly RazorCodeDocument _codeDocument;

    public DefaultRazorCSharpDocument(
        RazorCodeDocument codeDocument,
        string generatedCode,
        RazorCodeGenerationOptions options,
        RazorDiagnostic[] diagnostics,
        ImmutableArray<SourceMapping> sourceMappings,
        ImmutableArray<SourceSpan> generatedOnlyMappings,
        LinePragma[] linePragmas)
    {
        if (generatedCode == null)
        {
            throw new ArgumentNullException(nameof(generatedCode));
        }

        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _codeDocument = codeDocument;
        _generatedCode = generatedCode;
        _options = options;

        _diagnostics = diagnostics ?? Array.Empty<RazorDiagnostic>();
        _sourceMappings = sourceMappings;
        _generatedOnlyMappings = generatedOnlyMappings;
        _linePragmas = linePragmas ?? Array.Empty<LinePragma>();
    }

    public override IReadOnlyList<RazorDiagnostic> Diagnostics => _diagnostics;

    public override string GeneratedCode => _generatedCode;

    public override ImmutableArray<SourceMapping> SourceMappings => _sourceMappings;

    public override ImmutableArray<SourceSpan> GeneratedOnlyMappings => _generatedOnlyMappings;

    internal override IReadOnlyList<LinePragma> LinePragmas => _linePragmas;

    public override RazorCodeGenerationOptions Options => _options;

    public override RazorCodeDocument CodeDocument => _codeDocument;
}
