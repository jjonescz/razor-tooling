// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Test;

public abstract class IntermediateNodeTestBase
{
    // Needed to resolve ambiguity between Microsoft.AspNetCore.Html namespace and IntermediateNodeAssert.Html method.
    protected static void Html(string expected, IntermediateNode node)
        => IntermediateNodeAssert.Html(expected, node);

}
