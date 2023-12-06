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
    private readonly ImmutableArray<SourceSpan> _componentMappings;
    private readonly LinePragma[] _linePragmas;
    private readonly RazorCodeGenerationOptions _options;
    private readonly RazorCodeDocument _codeDocument;

    public DefaultRazorCSharpDocument(
        RazorCodeDocument codeDocument,
        string generatedCode,
        RazorCodeGenerationOptions options,
        RazorDiagnostic[] diagnostics,
        ImmutableArray<SourceMapping> sourceMappings,
        ImmutableArray<SourceSpan> componentMappings,
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
        _componentMappings = componentMappings;
        _linePragmas = linePragmas ?? Array.Empty<LinePragma>();
        _componentMappings = componentMappings;
    }

    public override IReadOnlyList<RazorDiagnostic> Diagnostics => _diagnostics;

    public override string GeneratedCode => _generatedCode;

    public override ImmutableArray<SourceMapping> SourceMappings => _sourceMappings;

    public override ImmutableArray<SourceSpan> ComponentMappings => _componentMappings;

    internal override IReadOnlyList<LinePragma> LinePragmas => _linePragmas;

    public override RazorCodeGenerationOptions Options => _options;

    public override RazorCodeDocument CodeDocument => _codeDocument;
}
